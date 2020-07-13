/*********************************************************************************************
   Copyright 2017 Duke Energy Corporation and Open Energy Solutions, Inc.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
**********************************************************************************************/

using System;
using System.Windows.Forms;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using EA;

namespace EAProtobufExporter
{
    public class Main
    {
        private static Boolean DEBUG = false;

        const String menuHeader = "-&ProtoBuf Generator";
        const String menuItem_Generate_proto3 = "&Generate proto3...";
        const String menuItem_About = "&About";

        private static String logFileName = null;

        // Tree View Processing Variables
        private static OpenFMBMessageProfileSelector profileSelector = null;
        private static TreeNodeCollection treeNodes = null;
        private static TreeNode parentPackageNode = null;
        private static HashSet<String> unnestedElements = null;
        private static Boolean checkModelNode = false;
        private static Boolean checkParentPackageNode = false;
        private static Boolean checkPackageNode = false;

        // Generate Protobuf Variables
        private static Proto3ModulelInfo proto3GlobalModuleInfo = null;
        private static HashSet<int> importPackageIDs = null;
        private static List<String> proto3FileNames = null;
        private static List<Proto3File> proto3Files = null;
        private static String childPackageName = null;
        private static Boolean recursiveProcessing = true;
 
        public String EA_Connect(Repository repository)
        {
            return "A non-specialized Add-In";

        } // end of public String EA_Connect(Repository repository)

        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

        } // end of public void EA_Disconnect()

        public object EA_GetMenuItems(Repository repository, String menuLocation, String menuName)
        {
            switch (menuName)
            {
                case "":
                    return menuHeader;

                case menuHeader:
                    string[] subMenus = { menuItem_Generate_proto3, menuItem_About };
                    return subMenus;

            } // end switch
 
            return "";

        } // end of public object EA_GetMenuItems(Repository repository, String menuLocation, String menuName)

        public Boolean IsProjectOpen(Repository repository)
        {
            Boolean projectOpen = true;

            try
            {
                Collection c = repository.Models;
            }
            catch
            {
                projectOpen = false;
            } // end catch

            return projectOpen;

        } // end of public Boolean IsProjectOpen(Repository repository)

        public void EA_GetMenuState(Repository repository, String menuLocation, String menuName, String menuItemName, ref Boolean isMenuEnabled, ref Boolean isMenuChecked)
        {
            if (IsProjectOpen(repository))
            {
                Global.umlFileName = repository.ConnectionString.Substring(repository.ConnectionString.LastIndexOf("\\") + 1);
                logFileName = repository.ConnectionString.Substring(0, repository.ConnectionString.LastIndexOf("."));
                switch (menuItemName)
                {
                    case menuItem_Generate_proto3:
                        isMenuEnabled = true;
                        break;

                    case menuItem_About:
                        isMenuEnabled = true;
                        break;

                    default:
                        isMenuEnabled = false;
                        break;

                } // end switch
            }
            else
            {
                isMenuEnabled = false;
            } // end else

        } // end of public void EA_GetMenuState(Repository repository, String menuLocation, String menuName, String menuItemName, ref Boolean isMenuEnabled, ref Boolean isMenuChecked)
 
        public void EA_MenuClick(EA.Repository repository, String menuLocation, String menuName, String menuItemName)
        {
            switch (menuItemName)
            {
                case menuItem_Generate_proto3:
                    Global.repository = repository;
                    Global.userAction = new UserAction();
                    profileSelector = new OpenFMBMessageProfileSelector();
                    Global.textBoxOutput = new TextBoxOutput(profileSelector.getTextBox());
                    Global.checkedElements = new HashSet<string>();
                    unnestedElements = new HashSet<string>();
                    Global.hideCheckBoxList = new HashSet<string>();
                    populateTreeView(profileSelector);
                    profileSelector.Show();
                    profileSelector.whatNodesAreChecked();
                    break;

                case menuItem_About:
                    AboutBox aboutBox = new AboutBox();
                    aboutBox.ShowDialog();
                    break;

            } // end switch

        } // end of public void EA_MenuClick(EA.Repository repository, String menuLocation, String menuName, String menuItemName)

        internal static void populateTreeView(OpenFMBMessageProfileSelector profileSelector)
        {
            treeNodes = profileSelector.getTreeView().Nodes;

            foreach (Package model in Global.repository.Models)
            {
                TreeNode newNode = new TreeNode(model.Name);
                newNode.Expand();
                newNode.Tag = model.Name;
                treeNodes.Add(newNode);

                foreach (Package package in model.Packages)
                {
                    checkParentPackageNode = false;
                    populateTreeNodes(newNode, package, true);
                    newNode.Checked = checkPackageNode;
                    checkPackageNode = false;
                } // end foreach

                newNode.Checked = checkModelNode;
                if (newNode.Checked)
                {
                    Global.checkedElements.Add(model.Name);
                    removeFromHideCheckBoxList(model.Name);
                } // end if

            } // end foreach

        } // end of internal static void populateTreeView(OpenFMBMessageProfileSelector profileSelector)

        private static void populateTreeNodes(TreeNode parentNode, Package currentPackage, Boolean isParentPackage)
        {
            TreeNode packageNode = new TreeNode(currentPackage.Name);
            packageNode.Tag = currentPackage.Name;
            parentNode.Nodes.Add(packageNode);

            if (isParentPackage)
            {
                parentPackageNode = packageNode;
            } // end if

            foreach (Package childPackage in currentPackage.Packages)
            {
                populateTreeNodes(packageNode, childPackage, false);
            } // end foreach

            populatePackageClassesAndEnumerations(packageNode, currentPackage);
            if (packageNode.Checked)
            {
                packageNode.Expand();
                checkParentNode(packageNode);
                Global.checkedElements.Add(packageNode.Text);
                removeFromHideCheckBoxList(packageNode.Text);
            }
            else
            {
                profileSelector.hideCheckBox(packageNode);
                profileSelector.hideCheckBox(packageNode);
                Global.hideCheckBoxList.Add(packageNode.Text);
            } // end else

            parentPackageNode.Checked = checkParentPackageNode;
            if (parentPackageNode.Checked)
            {
                Global.checkedElements.Add(parentPackageNode.Text);
                removeFromHideCheckBoxList(parentPackageNode.Text);
            }
            else
            {
                profileSelector.hideCheckBox(parentPackageNode);
                Global.hideCheckBoxList.Add(parentPackageNode.Text);
            } // end else

            checkPackageNode = false;

        } // end of private static void populateTreeNodes(TreeNode parentNode, Package currentPackage, Boolean isParentPackage)

        private static void checkParentNode(TreeNode currentNode)
        {
            TreeNode parentNode = currentNode.Parent;
            if (parentNode != null)
            {
                parentNode.Checked = true;
                Global.checkedElements.Add(parentNode.Text);
                removeFromHideCheckBoxList(parentNode.Text);
                checkParentNode(parentNode);
            } // end if

        } // end of private static void checkParentNode(TreeNode currentNode)

        private static void populatePackageClassesAndEnumerations(TreeNode parentNode, Package currentPackage)
        {
            Boolean nestedFound = false;
            Boolean checkNode = false;

            foreach (Element element in currentPackage.Elements)
            {
                foreach (TaggedValue taggedValue in element.TaggedValues)
                {
                    if (taggedValue.Name.Equals("nested"))
                    {
                        nestedFound = true;
                        if (taggedValue.Value.ToUpper().Equals("FALSE"))
                        {
                            checkNode = true;
                            checkPackageNode = true;
                            checkParentPackageNode = true;
                            checkModelNode = true;
                            parentNode.Checked = true;
                            unnestedElements.Add(element.Name);
                            String name = taggedValue.Name;
                            String value = taggedValue.Value;
                        } // end if

                    } // end if

                    if (taggedValue.Name.Equals("ProtobufTag_extend"))
                    {
                        nestedFound = true;
                        if (taggedValue.Value.ToUpper().Equals("TRUE"))
                        {
                            checkNode = true;
                            checkPackageNode = true;
                            checkParentPackageNode = true;
                            checkModelNode = true;
                            parentNode.Checked = true;
                            unnestedElements.Add(element.Name);
                            String name = taggedValue.Name;
                            String value = taggedValue.Value;
                        } // end if

                    } // end if

                } // end foreach

                if (isClass(element) || isEnumeration(element))
                {
                    TreeNode classNode = new TreeNode(element.Name);
                    classNode.Checked = false;
                    parentNode.Nodes.Add(classNode);
                    if (!checkNode || !nestedFound)
                    {
                        classNode.ForeColor = Color.Gray;
                    } // end if

                    profileSelector.hideCheckBox(classNode);
                    Global.hideCheckBoxList.Add(classNode.Text);

                } // end if

                checkNode = false;

            } // end foreach

        } // end of private static void populatePackageClassesAndEnumerations(TreeNode parentNode, Package currentPackage)

        private static Boolean isClass(Element element)
        {
            return (element.Type == "Class");

        } // end of private static Boolean isClass(Element element)

        private static Boolean isEnumeration(Element element)
        {
            return element.Type.Equals("Enumeration") || element.Stereotype.Equals("enumeration");

        } // end of private static Boolean isElementEnum(Element element)

        private static Boolean isUMLDiagram(Element element)
        {
            return (element.Type == "UMLDiagram");

        } // end of private static Boolean isUMLDiagram(Element element)

        private static void removeFromHideCheckBoxList(String nodeName)
        {
            if (Global.hideCheckBoxList.Contains(nodeName))
            {
                Global.hideCheckBoxList.Remove(nodeName);
            } // end if

        } // end of private static void removeFromHideCheckBoxList(String nodeName)

        private static void initializeGenerateProto3Variables()
        {
            Global.textBoxOutput.clear();
            Global.warningMessages = new List<string>();
            Global.informationalMessages = new List<string>();
            Global.errorMessages = new List<string>();
            Global.errorGeneratingProtobuf = false;
            proto3GlobalModuleInfo = null;

            if (importPackageIDs != null)
            {
                importPackageIDs.Clear();
                importPackageIDs = null;
            } // end if

            importPackageIDs = new HashSet<int>();

            if (proto3FileNames != null)
            {
                proto3FileNames.Clear();
                proto3FileNames = null;
            } // end if

            proto3FileNames = new List<String>();

            if (proto3Files != null)
            {
                proto3Files.Clear();
                proto3Files = null;
            } // end if

            proto3Files = new List<Proto3File>();

            childPackageName = null;
            recursiveProcessing = true;

        } // end of private static void initializeGenerateProto3Variables()

        internal static void generateProto3()
        {
            initializeGenerateProto3Variables();
            foreach (Package model in Global.repository.Models)
            {
                if (Global.checkedElements.Contains(model.Name))
                {
                    foreach (Package parentPackage in model.Packages)
                    {
                        if (Global.checkedElements.Contains(parentPackage.Name))
                        {
                            processPackage(parentPackage);
                        } // end if

                    } // end foreach

                } // end if

            } // end foreach

            HashSet<int> tempImportPackageIDs = new HashSet<int>();
            foreach (int importPackageID in importPackageIDs)
            {
                tempImportPackageIDs.Add(importPackageID);
            } // end foreach

            foreach (int importPackageID in tempImportPackageIDs)
            {
                Package importPackage = Global.repository.GetPackageByID(importPackageID);
                if (importPackage != null)
                {
                    Boolean found = false;
                    int i = 0;
                    while (!found && i < proto3Files.Count)
                    {
                        Proto3File proto3File = proto3Files[i];
                        if (importPackage.Name.Equals(proto3File.getPackageName()))
                        {
                            found = true;
                        } // end if

                        i++;

                    } // end while

                    if (!found)
                    {
                        childPackageName = importPackage.Name;
                        Proto3File proto3File = buildProto3File(importPackage);
                        proto3File.setSelectedPackage(true);
                        foreach (Element element in importPackage.Elements)
                        {
                            recursiveProcessing = false;
                            processPackageElement(element, proto3File);
                        } // end foreach

                    } // end if

                } // end if

            } // end foreach

            importPackageIDs.Clear();
            importPackageIDs = null;

            if (DEBUG)
            {
                proto3GlobalModuleInfo.print(0);
                foreach (Proto3File proto3File in proto3Files)
                {
                    proto3File.print(0);
                } // end foreach

            } // end if

            if (Global.informationalMessages.Count > 0)
            {
                outputProcessingMessages(Global.informationalMessages);
            } // end if

            if (Global.warningMessages.Count > 0)
            {
                outputProcessingMessages(Global.warningMessages);
            } // end if

            if (Global.errorMessages.Count > 0)
            {
                outputProcessingMessages(Global.errorMessages);
            } // end if

            if (Global.errorGeneratingProtobuf)
            {
                foreach (TreeNode model in treeNodes)
                {
                    profileSelector.setCheckBoxOfAllChildNodes(model, false);
                } // end foreach

                profileSelector.disableGenerateButton();

                Global.textBoxOutput.outputTextLine();
                Global.textBoxOutput.outputTextLine("An error was encountered while generating the proto3 files.");
                Global.textBoxOutput.outputTextLine("The saving of the proto3 files has been disabled.");
                Global.textBoxOutput.outputTextLine("Please correct the error(s) listed, reselect the packages/modules to generate, and regenerate the proto3 files.");
            } // end if

            Global.protoFileGenerationDateTime = DateTime.Now.ToString("U") + " UTC";
            Global.textBoxOutput.writeLogFile(logFileName + " - " + DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss") + ".log");

        } // end of internal static void generateProto3()

        private static void processPackage(Package package)
        {
            Proto3File proto3File = null;

            if (package.Element.TaggedValues.Count > 0)
            {
                Proto3ModulelInfo proto3ModuleInfo = processPackageTaggedValues(package);
                if (proto3ModuleInfo.getPackageValue() != null)
                {
                    proto3GlobalModuleInfo = proto3ModuleInfo;
                    if (!proto3GlobalModuleInfo.isValid())
                    {
                        Global.errorGeneratingProtobuf = true;
                    } // end if

                }
                else
                {
                    childPackageName = package.Name;
                    proto3File = buildProto3File(package);
                    proto3File.setSelectedPackage(true);
                    proto3File.setProto3ModulelInfo(proto3ModuleInfo);
                    proto3File.setComment(package.Notes.Trim());
                } // end else

            } // end if

            if (package.Elements.Count > 0)
            {
                if (proto3GlobalModuleInfo != null && proto3GlobalModuleInfo.getProto3MessageEnumerations().Count == 0)
                {
                    // processGlobalElements now returns multiple proto3MessageEnumerations since we added "option_openfmb_profile"
                    List<Proto3MessageEnumeration> proto3MessageEnumerations = processGlobalElements(package);
                    foreach(Proto3MessageEnumeration enumeration in proto3MessageEnumerations)
                    {
                        proto3GlobalModuleInfo.addProto3MessageEnumeration(enumeration);
                    }

                }
                else if (proto3File != null)
                {
                    foreach (Element element in package.Elements)
                    {
                        processPackageElement(element, proto3File);
                    } // end foreach

                } // end else

            } // end if

            if (package.Packages.Count > 0)
            {
                foreach (Package childPackage in package.Packages)
                {
                    if (Global.checkedElements.Contains(childPackage.Name))
                    {
                        processPackage(childPackage);
                    } //end if

                } // end foreach

            } // end if

        } // end of private static void processPackage(Package package)

        private static Proto3ModulelInfo processPackageTaggedValues(Package package)
        {
            Proto3ModulelInfo proto3ModulelInfo = new Proto3ModulelInfo(package.Name);
            foreach (TaggedValue taggedValue in package.Element.TaggedValues)
            {
                switch (taggedValue.Name)
                {
                    case Proto3ModulelInfo.GO_PACKAGE_OPTION_NAME:
                        proto3ModulelInfo.setGoPackageOptionValue(taggedValue.Value);
                        break;

                    case Proto3ModulelInfo.JAVA_PACKAGE_OPTION_NAME:
                        proto3ModulelInfo.setJavaPackageOptionValue(taggedValue.Value);
                        break;

                    case Proto3ModulelInfo.JAVA_MULTIPLE_FILES_OPTION_NAME:
                        proto3ModulelInfo.setJavaMultipleFilesOptionValue(taggedValue.Value);
                        break;

                    case Proto3ModulelInfo.CSHARP_NAMESPACE_OPTION_NAME:
                        proto3ModulelInfo.setCSharpNamespaceOptionValue(taggedValue.Value);
                        break;

                    case Proto3ModulelInfo.PACKAGE_NAME:
                        proto3ModulelInfo.setPackageValue(taggedValue.Value);
                        break;

                } // end switch

            } // end foreach

            return proto3ModulelInfo;

        } // end of private static Proto3ModulelInfo processPackageTaggedValues(Package package)

        private static List<Proto3MessageEnumeration> processGlobalElements(Package parentPackage)
        {
            List<Proto3MessageEnumeration> list = new List<Proto3MessageEnumeration>();

            foreach (Element element in parentPackage.Elements)
            {
                Proto3MessageEnumeration proto3MessageEnumeration = null;

                foreach (TaggedValue taggedValue in element.TaggedValues)
                {
                    var tagName = taggedValue.Name;
                    System.Diagnostics.Debug.WriteLine(taggedValue.Name + " = " + taggedValue.Value);
                    if (taggedValue.Name.Equals("ProtobufTag_extend") && taggedValue.Value.Equals("true") && proto3GlobalModuleInfo != null)
                    {
                        proto3GlobalModuleInfo.setStereotype(element.Stereotype);
                        proto3MessageEnumeration = new Proto3MessageEnumeration("extend", element.Stereotype + "." + element.Name);
                    } // foreach

                } // end foreach

                if (proto3MessageEnumeration != null)
                {
                    foreach (EA.Attribute attribute in element.Attributes)
                    {
                        Proto3Field proto3Field = new Proto3Field(attribute.Type, attribute.Name);
                        if (DataTypeConversion.checkInvalidDataType(attribute.Type))
                        {
                            Global.errorMessages.Add("ERROR - Main.ProcessGlobalElements - " + Global.INVALID_PROTOBUF_DATATYPE + " for Attribute Name '" + attribute.Name + "' with Attribute Type '" + attribute.Type + "' in Element '" + element.Name + "'.");
                            Global.errorGeneratingProtobuf = true;
                        } // end if

                        int defaultValue = 0;
                        if (!Int32.TryParse(attribute.Default, out defaultValue))
                        {
                            Global.errorMessages.Add("ERROR - Main.ProcessGlobalElements - The default value is invalid for Attribute '" + attribute.Name + "'.");
                            Global.errorGeneratingProtobuf = true;
                        }
                        else
                        {
                            proto3Field.setDefaultValue(defaultValue);
                        }

                        proto3Field.setComment(attribute.Notes.Trim());
                        if (proto3Field != null)
                        {
                            proto3MessageEnumeration.AddProto3Field(proto3Field);

                        } // end if

                    } // end foreach

                    if (!proto3MessageEnumeration.sortProto3Fields())
                    {
                        Global.errorGeneratingProtobuf = true;
                    } // end if

                    list.Add(proto3MessageEnumeration);

                } // end if

            } // end foreach           

            return list;

        } // end of private static List<Proto3MessageEnumeration> processGlobalElements(Package parentPackage)

        private static void processPackageElement(Element element, Proto3File proto3File)
        {
            Proto3MessageEnumeration proto3MessageEnumeration = null;

            if (!recursiveProcessing)
            {
                proto3MessageEnumeration = buildProto3MessageEnumeration(element, proto3File);
            }
            else if (unnestedElements.Contains(element.Name))
            {
                proto3MessageEnumeration = buildProto3MessageEnumeration(element, proto3File);
            } // end else if

            if (proto3MessageEnumeration != null)
            {
                proto3MessageEnumeration.setComment(element.Notes.Trim());
                proto3File.AddProto3MessageEnumeration(proto3MessageEnumeration);
            } // end if

        } // end of private static void processPackageElement(Element element, Proto3File proto3File)

        private static Proto3File buildProto3File(Package package)
        {
            Proto3File proto3File = null;

            if (!proto3FileNames.Contains(package.Name))
            {
                String parentPackageName = null;
                if (package.ParentID != 0)
                {
                    Package parentPackage = Global.repository.GetPackageByID(package.ParentID);
                    parentPackageName = parentPackage.Name;
                } // end if

                proto3File = new Proto3File(parentPackageName, package.Name);
                proto3Files.Add(proto3File);
                proto3FileNames.Add(package.Name);
                if (importPackageIDs.Contains(package.PackageID))
                {
                    importPackageIDs.Remove(package.PackageID);
                } // end if

            } // end if

            return proto3File;

        } // end of private static Proto3File buildProto3File(Package package)

        private static Proto3MessageEnumeration buildProto3MessageEnumeration(Element element, Proto3File proto3File)
        {
            Proto3MessageEnumeration proto3MessageEnumeration = null;
            if (proto3File.findProto3MessageEnumeration(element.Name) == null)
            {
                String type = null;
                if (element.Type.Equals("Class"))
                {
                    type = "message";
                }
                else if (element.Type.Equals("Enumeration"))
                {
                    type = "enum";
                }
                else
                {
                    Global.informationalMessages.Add("INFORMATION - Main.BuildProto3MessageEnumeration - Element Type '" + element.Type + "' of Element '" + element.Name + "' in Package '" + proto3File.getPackageName() + "' will not be processed.");
                } // end else

                if (type != null)
                {
                    proto3MessageEnumeration = new Proto3MessageEnumeration(type, element.Name);

                    foreach (EA.TaggedValue tag in element.TaggedValues)
                    {
                        var tagName = tag.Name;
                        System.Diagnostics.Debug.WriteLine(tag.Name + " = " + tag.Value);
                        if (tag.Name.Equals("ProtobufTag_openfmb_profile") && tag.Value.ToUpper().Equals("TRUE"))
                        {
                            if (type == "message")
                            {
                                proto3MessageEnumeration.setOpenFmbProfile(true);
                            }
                        } // end if
                        else if (tag.Name.Equals("ProtobufTag_Reserved") && !string.IsNullOrWhiteSpace(tag.Value))
                        {
                            proto3MessageEnumeration.setReservedTags(tag.Value);
                        }
                    }

                    proto3MessageEnumeration.setComment(element.Notes.Trim());

                    List<Proto3Field> proto3Fields = buildProto3Fields(element, proto3File);
                    if (proto3Fields != null)
                    {
                        proto3MessageEnumeration.setProto3FieldsList(proto3Fields);
                        if (!proto3MessageEnumeration.sortProto3Fields())
                        {
                            Global.errorGeneratingProtobuf = true;
                        } // end if

                    } // end if

                } // end if

            } // end if

            return proto3MessageEnumeration;

        } // end of private static Proto3MessageEnumeration buildProto3MessageEnumeration(Element element, Proto3File proto3File)

        private static List<Proto3Field> buildProto3Fields(Element element, Proto3File proto3File)
        {
            List<Proto3Field> proto3Fields = new List<Proto3Field>();

            foreach (EA.Attribute attribute in element.Attributes)
            {
                Proto3Field proto3Field = processElementAttributes(element, attribute, proto3File);
                if (proto3Field != null)
                {
                    proto3Fields.Add(proto3Field);
                } // end if

            } // end foreach

            List<String> proto3Connectors = new List<String>();

            foreach (Connector connector in element.Connectors)
            {
                Element connectorSupplierElement = Global.repository.GetElementByID(connector.SupplierID);
                if (!connectorSupplierElement.Name.Equals(element.Name))
                {
                    Proto3Field proto3Field = processConnector(connector, proto3File);
                    if (proto3Field != null)
                    {
                        proto3Fields.Add(proto3Field);
                    } // end if

                } // end if

            } // end foreach

            return proto3Fields;

        } // end of private static List<Proto3Field> buildProto3Fields(Element element, Proto3File proto3File)

        private static Proto3Field processElementAttributes(Element element, EA.Attribute attribute, Proto3File proto3File)
        {
            Element elementOfAttribute = null;

            Proto3Field proto3Field = new Proto3Field(attribute.Type, attribute.Name);
            if (DataTypeConversion.checkInvalidDataType(attribute.Type))
            {
                Global.errorMessages.Add("ERROR - Main.ProcessElementAttributes - " + Global.INVALID_PROTOBUF_DATATYPE + " for Attribute Name '" + attribute.Name + "' with Attribute Type '" + attribute.Type + "' in Element '" + element.Name + "'.");
                Global.errorGeneratingProtobuf = true;
            } // end if

            proto3Field.setComment(attribute.Notes.Trim());
            int multiplicity = 0;
            if (!Int32.TryParse(attribute.LowerBound, out multiplicity))
            {
                Global.errorMessages.Add("ERROR - Main.ProcessElementAttributes - Lower Bound Multiplicity is invalid for Attribute '" + attribute.Name + "'.");
                Global.errorGeneratingProtobuf = true;
            }
            else
            {
                proto3Field.setMinMultiplicity(multiplicity);
            } // end else

            if (attribute.UpperBound.Equals("*"))
            {
                proto3Field.setInfiniteMaxMultiplicity(true);
            }
            else if (!Int32.TryParse(attribute.UpperBound, out multiplicity))
            {
                Global.errorMessages.Add("ERROR - Main.ProcessElementAttributes - Upper Bound Multiplicity is invalid for Attribute '" + attribute.Name + "'.");
                Global.errorGeneratingProtobuf = true;
            }
            else
            {
                proto3Field.setMaxMultiplicity(multiplicity);
            } // end else

            if (attribute.ClassifierID != 0)
            {
                try
                {
                    elementOfAttribute = Global.repository.GetElementByID(attribute.ClassifierID);
                    if (elementOfAttribute != null)
                    {
                        if ((Global.repository.GetPackageByID(elementOfAttribute.PackageID)).Name.Equals(proto3File.getPackageName()))
                        {
                            Proto3MessageEnumeration proto3MessageEnumeration = buildProto3MessageEnumeration(elementOfAttribute, proto3File);
                            if (proto3MessageEnumeration != null)
                            {
                                proto3File.AddProto3MessageEnumeration(proto3MessageEnumeration);
                            } // end if

                        } // end if

                        foreach (EA.TaggedValue classifierIDTaggedValue in elementOfAttribute.TaggedValues)
                        {
                            if (classifierIDTaggedValue.Name.Equals("ProtobufTag_UUID") && classifierIDTaggedValue.Value.ToUpper().Equals("TRUE"))
                            {
                                proto3Field.setUUID(true);
                            } // end if

                        } // end foreach

                        String baseDataType = getBaseDataType(elementOfAttribute);
                        if (baseDataType != null)
                        {
                            proto3Field.setVariableType(baseDataType);
                            proto3Field.setExternalPackageName(null);
                            if (proto3Field.getMinMultiplicity() == 0 && (baseDataType.ToLower().Equals("enumeration") || baseDataType.ToLower().Equals("enum")))
                            {
                                proto3Field.setOptionalEnumerationWrapperSet(true);
                            } // end if

                        }
                        else if (!proto3File.getPackageName().Equals((Global.repository.GetPackageByID(elementOfAttribute.PackageID)).Name))
                        {
                            proto3Field.setExternalPackageName((Global.repository.GetPackageByID(elementOfAttribute.PackageID)).Name);
                            if (proto3Field.getMinMultiplicity() == 0 && elementOfAttribute.Type.Equals("Enumeration"))
                            {
                                proto3Field.setOptionalEnumerationWrapperSet(true);
                            } // end if

                        }
                        else if (proto3Field.getMinMultiplicity() == 0 && elementOfAttribute.Type.Equals("Enumeration"))
                        {
                            proto3Field.setOptionalEnumerationWrapperSet(true);
                        } // end else if

                        if (!recursiveProcessing)
                        {
                            if (proto3Field.getExternalPackageName() != null)
                            {
                                Global.warningMessages.Add("WARNING - Main.ProcessElementAttributes - The attribute type '" + proto3Field.getExternalPackageName() + "." + proto3Field.getVariableType() + "' was detected in the package '" + proto3File.getPackageName() + "' for attribute '" + proto3Field.getVariableName() + "' of element '" + element.Name + "'.");
                            } // end if

                        } // end if

                    } // end if

                }
                catch (COMException e)
                {
                    Global.errorMessages.Add("ERROR - Main.ProcessElementAttributes - COMException caught in ProcessElementAttributes while processing Attribute '" + attribute.Name + "'.");
                    Global.errorMessages.Add(e.Message);
                    Global.errorGeneratingProtobuf = true;
                } // end catch

            } // end if

            if (attribute.Stereotype.Equals("enum"))
            {
                int defaultValue = 0;
                if (!Int32.TryParse(attribute.Default, out defaultValue))
                {
                    Global.errorMessages.Add("ERROR - Main.ProcessElementAttributes - The default value for the enumeration is invalid for Attribute '" + attribute.Name + "'.");
                    Global.errorGeneratingProtobuf = true;
                }
                else
                {
                    proto3Field.setDefaultValue(defaultValue);
                } // end else

            }
            else
            {
                foreach (EA.AttributeTag attributeTag in attribute.TaggedValues)
                {
                    if (attributeTag.Name.Equals("ProtobufTag"))
                    {
                        int defaultValue = 0;
                        if (!Int32.TryParse(attributeTag.Value, out defaultValue))
                        {
                            Global.errorMessages.Add("ERROR - Main.ProcessElementAttributes - ProtobufTag is invalid for Attribute '" + attribute.Name + "'.");
                            Global.errorGeneratingProtobuf = true;
                        }
                        else
                        {
                            proto3Field.setDefaultValue(defaultValue);
                        } // end else

                        continue;

                    } // end if

                    if (attributeTag.Name.Equals("ProtobufTag_Key") && attributeTag.Value.ToUpper().Equals("TRUE"))
                    {
                        proto3Field.setKey(true);
                        continue;

                    } // end if

                } // end foreach

            } // end else

            return proto3Field;

        } // end of private static Proto3Field processElementAttributes(Element element, EA.Attribute attribute, Proto3File proto3File)

        private static String getBaseDataType(Element element)
        {
            String baseClassName = null;
            String parentIdentifier = "Parent=";
            String genLinks = element.Genlinks;
            if (genLinks != null && genLinks.StartsWith(parentIdentifier) && genLinks.Length > parentIdentifier.Length)
            {
                int parentBeginIndex = genLinks.IndexOf(parentIdentifier);

                if (parentBeginIndex >= 0)
                {
                    parentBeginIndex += parentIdentifier.Length;
                    int parentEndIndex = genLinks.IndexOf(";", parentBeginIndex);
                    if (parentEndIndex < 0)
                    {
                        parentEndIndex = genLinks.Length;
                    } // end if

                    baseClassName = genLinks.Substring(parentBeginIndex, parentEndIndex - parentBeginIndex);
                    String baseDataType = DataTypeConversion.getBaseDataType(baseClassName);
                    if (baseDataType != null)
                    {
                        baseClassName = baseDataType;
                    } // end if

                } // end if

            } // end if

            return baseClassName;

        } // end of private static String getBaseDataType(Element element)

        private static Proto3Field processConnector(Connector connector, Proto3File proto3File)
        {
            Proto3Field proto3Field = null;
            Boolean continueProcessing = true;

            Element connectorSupplierElement = Global.repository.GetElementByID(connector.SupplierID);
            if (connectorSupplierElement != null)
            {
                Package package = Global.repository.GetPackageByID(connectorSupplierElement.PackageID);
                if (package != null)
                {
                    if (!connector.Stereotype.Equals("trace"))
                    {
                        if (connector.Type.Equals("Association"))
                        {
                            proto3Field = processAssociation(package, connectorSupplierElement, connector);
                        }
                        else if (connector.Type.Equals("Generalization"))
                        {
                            proto3Field = processGeneralization(package, connectorSupplierElement, connector);
                        }
                        else
                        {
                            Global.warningMessages.Add("WARNING - Main.ProcessConnector - Connector Type '" + connector.Type + "' not processed.");
                        } // end else

                    }
                    else
                    {
                        continueProcessing = false;
                    } // end else

                    if (continueProcessing)
                    {
                        if (!package.Name.Equals(childPackageName))
                        {
                            if (importPackageIDs != null && recursiveProcessing)
                            {
                                if (!importPackageIDs.Contains(package.PackageID) && !proto3FileNames.Contains(package.Name))
                                {
                                    importPackageIDs.Add(package.PackageID);
                                } // end if

                                proto3File.AddImport(package.Name);
                                if (proto3Field != null)
                                {
                                    proto3Field.setExternalPackageName(package.Name);
                                } // end if

                            } // end if

                        }
                        else
                        {
                            Proto3MessageEnumeration proto3MessageEnumeration = buildProto3MessageEnumeration(connectorSupplierElement, proto3File);
                            if (proto3MessageEnumeration != null)
                            {
                                proto3File.AddProto3MessageEnumeration(proto3MessageEnumeration);
                            } // end if

                        } // end else

                    } // end if

                }
                else
                {
                    Global.errorMessages.Add("ERROR - Main.ProcessConnector - Package not found for Connector '" + connectorSupplierElement.Name + "'.");
                    Global.errorGeneratingProtobuf = true;
                } // end else

            }
            else
            {
                Global.errorMessages.Add("ERROR - Main.ProcessConnector - Connector Supplier Element not found.");
                Global.errorGeneratingProtobuf = true;
            } // end else

            return proto3Field;

        } // end of private static Proto3Field processConnector(Connector connector, Proto3File proto3File)

        private static Proto3Field processAssociation(Package package, Element element, Connector connector)
        {
            Proto3Field proto3Field = null;
            String variableName = null;
            Boolean found = false;

            variableName = connector.SupplierEnd.Role;
            proto3Field = new Proto3Field(element.Name, variableName);

            if (connector.SupplierEnd.Cardinality != null)
            {
                int multiplicity = 0;
                if (connector.SupplierEnd.Cardinality.Contains("."))
                {
                    String cardinality = connector.SupplierEnd.Cardinality;
                    if (!Int32.TryParse(cardinality.Substring(0, cardinality.IndexOf(".")), out multiplicity))
                    {
                        Global.errorMessages.Add("ERROR - Main.ProcessAssociation - The minimum cardinality is invalid for the Connector Supplier Element '" + element.Name + "' in Package '" + childPackageName + "'.");
                        Global.errorGeneratingProtobuf = true;
                    }
                    else
                    {
                        proto3Field.setMinMultiplicity(multiplicity);
                    } // end else

                    if ((cardinality.Substring(cardinality.LastIndexOf(".") + 1).Equals("*")))
                    {
                        proto3Field.setInfiniteMaxMultiplicity(true);
                    }
                    else if (!Int32.TryParse(cardinality.Substring(cardinality.LastIndexOf(".") + 1), out multiplicity))
                    {
                        Global.errorMessages.Add("ERROR - Main.ProcessAssociation - The maximum cardinality is invalid for the Connector Supplier Element '" + element.Name + "' in Package '" + childPackageName + "'.");
                        Global.errorGeneratingProtobuf = true;
                    }
                    else
                    {
                        proto3Field.setMaxMultiplicity(multiplicity);
                    } // end else

                }
                else
                {
                    if (!Int32.TryParse(connector.SupplierEnd.Cardinality, out multiplicity))
                    {
                        Global.errorMessages.Add("ERROR - Main.ProcessAssociation - The minimum cardinality is invalid for the Connector Supplier Element '" + element.Name + "' in Package '" + childPackageName + "'.");
                        Global.errorGeneratingProtobuf = true;
                    }
                    else
                    {
                        proto3Field.setMinMultiplicity(multiplicity);
                    } // end else

                } // end else

            } // end if

            foreach (RoleTag roleTag in connector.ClientEnd.TaggedValues)
            {
                if (roleTag.Tag.Equals("ProtobufTag"))
                {
                    int defaultValue = 0;
                    if (!Int32.TryParse(roleTag.Value, out defaultValue))
                    {
                        Global.errorMessages.Add("ERROR - Main.ProcessAssociation - ProtobufTag is invalid for the Connector Supplier Element '" + element.Name + "' in Package '" + childPackageName + "'.");
                        Global.errorGeneratingProtobuf = true;
                    }
                    else
                    {
                        proto3Field.setDefaultValue(defaultValue);
                    } // end else

                    found = true;
                    continue;

                } // end if

            } // end foreach

            if (!found)
            {
                Global.errorMessages.Add("ERROR - Main.ProcessAssociation - ProtobufTag not found for Association between '" + Global.repository.GetElementByID(connector.ClientID).Name + "' and '" + element.Name + "' in Package '" + childPackageName + "'.");
                Global.errorGeneratingProtobuf = true;
            } // end if

            return proto3Field;

        } // eend of private static Proto3Field processAssociation(Package package, Element element, Connector connector)

        private static Proto3Field processGeneralization(Package package, Element element, Connector connector)
        {
            Proto3Field proto3Field = null;
            String variableName = null;
            Boolean found = false;

            variableName = char.ToLower(element.Name[0]) + element.Name.Substring(1);
            proto3Field = new Proto3Field(element.Name, variableName);
            proto3Field.setParentMessage(true);

            foreach (ConnectorTag connectorTag in connector.TaggedValues)
            {
                if (connectorTag.Name.Equals("ProtobufTag"))
                {
                    int defaultValue = 0;
                    if (!Int32.TryParse(connectorTag.Value, out defaultValue))
                    {
                        Global.errorMessages.Add("ERROR - Main.ProcessGeneralization - ProtobufTag is invalid for the Connector Supplier Element '" + element.Name + "' in Package '" + childPackageName + "'.");
                        Global.errorGeneratingProtobuf = true;
                    }
                    else
                    {
                        proto3Field.setDefaultValue(defaultValue);
                    } // end else

                    found = true;
                    continue;

                } // end if

            } // end foreach

            if (!found)
            {
                Global.errorMessages.Add("ERROR - Main.ProcessGeneralization - ProtobufTag not found for Generalization between '" + Global.repository.GetElementByID(connector.ClientID).Name + "' and '" + element.Name + "' in Package '" + childPackageName + "'.");
                Global.errorGeneratingProtobuf = true;
            } // end if

            return proto3Field;

        } // end of private static Proto3Field processGeneralization(Repository repository, Package package, Element element, Connector connector)

        private static void outputProcessingMessages(List<String> messages)
        {
            foreach (String message in messages)
            {
                Global.textBoxOutput.outputTextLine(message);
            } // end foreach

            Global.textBoxOutput.outputTextLine();

        } // end of private static void outputProcessingMessages(List<String> messages)

        internal static void saveProto3(String selectedPath)
        {
            Global.textBoxOutput.clear();
            proto3GlobalModuleInfo.setSelectedPath(selectedPath);
            proto3GlobalModuleInfo.write();
            foreach (Proto3File proto3File in proto3Files)
            {
                if (proto3File.isSelectedPackage() || isPackageSelected(proto3File.getPackageName()))
                {
                    proto3File.setSelectedPath(selectedPath);
                    proto3File.write(proto3GlobalModuleInfo);
                } // end if

            } // end foreach

            Global.textBoxOutput.outputTextLine("Save complete...");

        } // end of internal static void saveProto3(String saveFileDirectoryName)

        private static Boolean isPackageSelected(String packageName)
        {
            Boolean packageFound = false;

            foreach (TreeNode model in treeNodes)
            {
                foreach (TreeNode parentPackage in model.Nodes)
                {
                    if (isChildPackageSelected(parentPackage, packageName))
                    {
                        packageFound = true;
                        continue;
                    } // end if

                } // end foreach

            } // end foreach

            return packageFound;

        } // end of private static Boolean isPackageSelected(String packageName)

        private static Boolean isChildPackageSelected(TreeNode treeNode, String packageName)
        {
            Boolean packageFound = false;

            foreach (TreeNode childPackage in treeNode.Nodes)
            {
                if (childPackage.Checked && childPackage.Text.Equals(packageName))
                {
                    packageFound = true;
                    break;
                } // end if

                if (isChildPackageSelected(childPackage, packageName))
                {
                    packageFound = true;
                    break;
                } // end if

            } // end foreach

            return packageFound;

        } // end of private static Boolean isChildPackageSelected(String packageName)

    } // end of public class Main

} // end of namespace EAProtobufExporter
