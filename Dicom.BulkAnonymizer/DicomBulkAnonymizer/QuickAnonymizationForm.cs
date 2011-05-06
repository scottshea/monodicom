using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Dicom;
using Dicom.Data;
using Monodicom.Utilities;

namespace DicomBulkAnonymizer
{
    public partial class QuickAnonymizationForm : Form
    {
        private DicomTag _patientNameTag = new DicomTag(DicomConstTags.PatientsName);
        private DicomTag _patientIdTag = new DicomTag(DicomConstTags.PatientID);
        private DicomTag _patientDOBTag = new DicomTag(DicomConstTags.PatientsBirthDate);
        private DicomTag _studyDescriptionTag = new DicomTag(DicomConstTags.StudyDescription);
        private DicomTag _studyDateTag = new DicomTag(DicomConstTags.StudyDate);
        private DicomTag _accessionNumberTag = new DicomTag(DicomConstTags.AccessionNumber);
        private DicomTag _referringPhysiciansNameTag = new DicomTag(DicomConstTags.ReferringPhysiciansName);

        private string _patientId;
        private string _accessionNumber;
        private DicomUID _studyID;

        private string _sourceDir = "";
        private string _targetDir = "";

        public QuickAnonymizationForm()
        {
            InitializeComponent();
            toolstripStatuslbl.Text = "Quick Anonymization Main Screen";
            txtPatientIdInput.Text = "12345";
            txtPatientNameInput.Text = "Anonymous^Patient";
            txtPatientDOBInput.Text = "19850101";
            txtStudyDescrptionInput.Text = "Anonymized Study";
            txtAccessionNumberInput.Text = "54321";
            txtStudyDateInput.Text = "19850101";
            txtReferringPhysicianNameInput.Text = "Anonymous^Doctor";
        }

        private void processStudies(string parentDir)
        {
            DicomUpdate dicomUpdate = new DicomUpdate();
            foreach (string dirs in Directory.GetDirectories(parentDir))
            {
                string dirName = Path.GetFileName(dirs);
                string targetOutputDirectory = _targetDir + "\\New_" + dirName;
                Directory.CreateDirectory(targetOutputDirectory);
                foreach (string file in Directory.GetFiles(dirs, "*", SearchOption.AllDirectories))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    toolstripStatuslbl.Text = "Copying file " + fileInfo.Name;
                    if (fileInfo.Extension == ".dcm")
                    {
                        fileInfo.CopyTo(targetOutputDirectory + "\\" + fileInfo.Name, true);
                    }
                }
                toolstripStatuslbl.Text = "Creating Update Objects";
                List<UpdateData> updateObjects = generateUpdateObjectList(targetOutputDirectory);
                foreach (UpdateData update in updateObjects)
                {
                    toolstripStatuslbl.Text = "Should be updating now...";
                    dicomUpdate.UpdateDicomFile(update);
                }
            }
            toolstripStatuslbl.Text = "Done";
        }

        private List<UpdateData> generateUpdateObjectList(string studyDir)
        {
            toolstripStatuslbl.Text = "Preparing files for anonymization";

            Random randomNumber = new Random();
            List<UpdateData> updateList = new List<UpdateData>();

            //Set Study ID
            _studyID = DicomUID.Generate();

            //Set Patient ID and Accession Number based on user random request
            if (chkbxRandomPatientId.Checked == true)
            {
                _patientId = randomNumber.Next(1000, 99999).ToString();
            }

            if (chkbxRandomAccessionNumber.Checked == true)
            {
                _accessionNumber = randomNumber.Next(1000, 99999).ToString();
            }

            foreach (string dcmFile in Directory.GetFiles(studyDir, "*", SearchOption.AllDirectories))
            {
                if(dcmFile.EndsWith(".dcm"))
                {
                    int seriesNumber = getSeriesNumberFromDcm(dcmFile);
                    updateList.Add(createUpdateObject(dcmFile, seriesNumber));
                }
            }
            return updateList;
        }

        private UpdateData createUpdateObject(string studyFilePath, int fileSeriesNumber)
        {
            UpdateData updateData = new UpdateData();
            DicomUID newSOPInstanceId = DicomUID.Generate(_studyID, fileSeriesNumber);
            DicomUID newSeriesInstanceId = DicomUID.Generate(_studyID, fileSeriesNumber);
            DicomUID newMediaStorageSOPInstanceId = DicomUID.Generate(_studyID, fileSeriesNumber);

            //Set File Path
            updateData.DicomFileName = studyFilePath;

            //Add in the user input
            updateData.UpdateDataset.Add(_patientNameTag, txtPatientNameInput.Text);
            updateData.UpdateDataset.Add(_patientIdTag, _patientId);
            updateData.UpdateDataset.Add(_patientDOBTag, txtPatientDOBInput.Text);
            updateData.UpdateDataset.Add(_studyDescriptionTag, txtStudyDescrptionInput.Text);
            updateData.UpdateDataset.Add(_studyDateTag, txtStudyDateInput.Text);
            updateData.UpdateDataset.Add(_accessionNumberTag, _accessionNumber);
            updateData.UpdateDataset.Add(_referringPhysiciansNameTag, txtReferringPhysicianNameInput.Text);
            updateData.UpdateDataset.Add(new DicomTag(DicomConstTags.StudyInstanceUID), _studyID.UID);
            updateData.UpdateDataset.Add(new DicomTag(DicomConstTags.SOPInstanceUID), newSOPInstanceId.UID);
            updateData.UpdateDataset.Add(new DicomTag(DicomConstTags.SeriesInstanceUID), newSeriesInstanceId.UID);

            //Add in the Meta Info
            updateData.UpdateMetadata.Add(new DicomTag(DicomConstTags.MediaStorageSOPInstanceUID), newMediaStorageSOPInstanceId.UID);

            return updateData;
        }

        private int getSeriesNumberFromDcm(string dcmFile)
        {
            int seriesNumber;
            DicomFileFormat fileRead = new DicomFileFormat();
            fileRead.Load(dcmFile, DicomReadOptions.DeferLoadingLargeElements);
            seriesNumber = Convert.ToInt32(fileRead.Dataset.GetValueString(new DicomTag(DicomConstTags.SeriesNumber)));
            fileRead = null;
            return seriesNumber;
        }

        private void btnChooseSourceDir_Click(object sender, EventArgs e)
        {
            dlgFolderChoose.ShowDialog();
            _sourceDir = dlgFolderChoose.SelectedPath;
            txtSourceDirInput.Text = dlgFolderChoose.SelectedPath;
            btnAnonymize.Enabled = true;
        }

        private void btnChooseTargetDir_Click(object sender, EventArgs e)
        {
            dlgFolderChoose.ShowDialog();
            _targetDir = dlgFolderChoose.SelectedPath;
            txtTargetDirInput.Text = dlgFolderChoose.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            toolstripStatuslbl.Text = "Closing Quick Anonymization Main Screen";
            this.Close();
        }

        private void btnAnonymize_Click(object sender, EventArgs e)
        {
            toolstripStatuslbl.Text = "Initializing Quick Anonymization";
            btnAnonymize.Enabled = false;
            if (_targetDir == "")
            {
                _targetDir = _sourceDir;
            }
            _patientId = txtPatientIdInput.Text;
            _accessionNumber = txtAccessionNumberInput.Text;
            processStudies(_sourceDir);
            btnAnonymize.Enabled = true;
            toolstripStatuslbl.Text = "Finished! No really... I am";
        }

        private void chkbxRandomPatientId_CheckedChanged(object sender, EventArgs e)
        {
            //toggle the input field
            if (txtPatientIdInput.Enabled == true)
            {
                txtPatientIdInput.Enabled = false;
            }
            else
            {
                txtPatientIdInput.Enabled = true;
            }
            toolstripStatuslbl.Text = "Toggling Patient ID input field";
        }

        private void chkbxRandomAccessionNumber_CheckedChanged(object sender, EventArgs e)
        {
            //toggle the input field
            if (txtAccessionNumberInput.Enabled == true)
            {
                txtAccessionNumberInput.Enabled = false;
            }
            else
            {
                txtAccessionNumberInput.Enabled = true;
            }
            toolstripStatuslbl.Text = "Toggling Accession Number input field";
        }
    }
}
