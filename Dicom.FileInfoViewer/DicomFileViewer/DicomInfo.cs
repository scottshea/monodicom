using System;
using System.Collections.Generic;
using Dicom;
using Dicom.Data;

namespace Monodicom.DicomInfoViewer
{
    public class DicomInfo
    {
        public List<Dictionary<string,string>> getDicomElementInfo(string fileName, string dirPath)
        {
            try
            {
                DicomFileFormat dff = new DicomFileFormat();
                List<Dictionary<string,string>> elements = new List<Dictionary<string,string>>();
                string loadPath = dirPath + "\\" + fileName;
                dff.Load(loadPath, DicomReadOptions.DeferLoadingLargeElements);
                foreach (DcmItem di in dff.Dataset.Elements)
                {
                    Dictionary<string,string> elementData = new Dictionary<string,string>();
                    elementData.Add("Name", di.Name.ToString());
                    elementData.Add("Tag", di.Tag.ToString());
                    if (dff.Dataset.GetValueString(di.Tag) != "")
                    {
                        elementData.Add("Value", dff.Dataset.GetValueString(di.Tag));
                    }
                    else
                    {
                        elementData.Add("Value", "--Null--");
                    }
                    elements.Add(elementData);
                }

                foreach(DcmItem di in dff.FileMetaInfo.Elements)
                {
                    Dictionary<string, string> elementData = new Dictionary<string, string>();
                    elementData.Add("Name", di.Name.ToString());
                    elementData.Add("Tag", di.Tag.ToString());
                    if (dff.FileMetaInfo.GetValueString(di.Tag) != "")
                    {
                        elementData.Add("Value", dff.FileMetaInfo.GetValueString(di.Tag));
                    }
                    else
                    {
                        elementData.Add("Value", "--Null--");
                    }
                    elements.Add(elementData);
                }

                return elements;
            }
            catch (Exception e)
            {
                string error = e.ToString();
                return null;
            }
        }

        public List<Dictionary<string, string>> getDicomElementInfo(string filePath)
        {
            try
            {
                DicomFileFormat dff = new DicomFileFormat();
                List<Dictionary<string, string>> elements = new List<Dictionary<string, string>>();
                string loadPath = filePath;
                dff.Load(loadPath, DicomReadOptions.DeferLoadingLargeElements);
                foreach (DcmItem di in dff.Dataset.Elements)
                {
                    Dictionary<string, string> elementData = new Dictionary<string, string>();
                    elementData.Add("Name", di.Name.ToString());
                    elementData.Add("Tag", di.Tag.ToString());
                    if (dff.Dataset.GetValueString(di.Tag) != "")
                    {
                        elementData.Add("Value", dff.Dataset.GetValueString(di.Tag));
                    }
                    else
                    {
                        elementData.Add("Value", "--Null--");
                    }
                    elements.Add(elementData);
                }

                foreach (DcmItem di in dff.FileMetaInfo.Elements)
                {
                    Dictionary<string, string> elementData = new Dictionary<string, string>();
                    elementData.Add("Name", di.Name.ToString());
                    elementData.Add("Tag", di.Tag.ToString());
                    if (dff.FileMetaInfo.GetValueString(di.Tag) != "")
                    {
                        elementData.Add("Value", dff.FileMetaInfo.GetValueString(di.Tag));
                    }
                    else
                    {
                        elementData.Add("Value", "--Null--");
                    }
                    elements.Add(elementData);
                }

                return elements;
            }
            catch (Exception e)
            {
                string error = e.ToString();
                return null;
            }
        }
    }
}
