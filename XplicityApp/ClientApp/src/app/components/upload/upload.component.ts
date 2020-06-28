import {Component, OnInit, Input, Inject} from '@angular/core';
import { HttpEventType } from '@angular/common/http';

import { AlertService } from 'src/app/services/alert.service';
import { FileType } from '../../enums/fileType';
import { FilesService } from 'src/app/services/files.service';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  @Input() fileType: FileType;
  fileName: string;
  progress: number;

  constructor(
    private alertService: AlertService,
    private fileService: FilesService,
    public dialogRef: MatDialogRef<UploadComponent>,
    @Inject(MAT_DIALOG_DATA) public acceptedFiles: string) { }

  ngOnInit() {
    this.fileType = FileType.HolidayPolicy;
  }

  closeUploadModal(){
    this.dialogRef.close();
  }

  public uploadFile = (files: string | any[]) => {
    if (files.length === 0) {
      return;
    }
    const fileToUpload = <File>files[0];
    const errorMessage = this.fileService.getValidationErrorMessage(fileToUpload, this.fileType);
    if (errorMessage !== '') {
      this.alertService.displayMessage(errorMessage);
      return;
    }
    this.fileName = fileToUpload.name;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('fileType', this.fileType.toString());

    this.fileService.upload(formData).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(100 * event.loaded / event.total);
      } else if (event.type === HttpEventType.Response) {
        this.alertService.displayMessage('You have successfully uploaded a file');
      }
    });
  }
}
