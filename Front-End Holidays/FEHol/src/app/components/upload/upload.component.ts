import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { FileType } from '../../helpers/file-type';
import * as mime from 'mime-types';
import { AlertService } from 'src/app/services/alert.service';
@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {

  public progress: number;
  public selectedFileType: string;
  public fileName: string;
  constructor(private http: HttpClient, private alertService: AlertService) { }

  ngOnInit() {
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    if (fileToUpload.size > 1048576) {
      this.alertService.displayMessage('The file is too large. File size limit is 10mb');
      return;
    }
    if (this.selectedFileType === FileType.holidayPolicy && fileToUpload.type !== mime.lookup('pdf')) {

      this.alertService.displayMessage('You can only upload pdf files for this option');
      return;
    }
    else if (this.selectedFileType === FileType.document) {
      if (fileToUpload.type !== mime.lookup('docx') && fileToUpload.type !== mime.lookup('doc')) {
        this.alertService.displayMessage('You can only upload doc and docx files for this option');
        return;
      }
    }
    else if (this.selectedFileType === FileType.image) {
      if (fileToUpload.type !== mime.lookup('jpg') && fileToUpload.type !== mime.lookup('png')) {
        this.alertService.displayMessage('You can only upload jpeg and png files for this option');
        return;
      }
    }
    this.fileName = fileToUpload.name;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('fileType', this.selectedFileType);

    this.http.post('https://localhost:44374/api/file/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total);
        }
        else if (event.type === HttpEventType.Response) {
          this.alertService.displayMessage('You have successfuly uploaded a file');
        }
      });
  }
}
