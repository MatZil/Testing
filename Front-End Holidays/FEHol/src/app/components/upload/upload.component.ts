import { Component, OnInit, Input } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import * as mime from 'mime-types';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from '../../../environments/environment';
import { FileType } from '../../enums/fileType';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  @Input()
  public fileType: number;
  public fileName: string;
  public progress: number;
  private readonly fileApi = `${environment.webApiUrl}/file`;
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
    if (this.fileType === FileType.HolidayPolicy && fileToUpload.type !== mime.lookup('pdf')) {

      this.alertService.displayMessage('You can only upload pdf files for holiday policy');
      return;
    }
    else if (this.fileType === FileType.Document) {
      if (fileToUpload.type !== mime.lookup('docx') && fileToUpload.type !== mime.lookup('doc')) {
        this.alertService.displayMessage('You can only upload doc and docx files for this option');
        return;
      }
    }
    else if (this.fileType === FileType.Image) {
      if (fileToUpload.type !== mime.lookup('jpg') && fileToUpload.type !== mime.lookup('png')) {
        this.alertService.displayMessage('You can only upload jpeg and png files for this option');
        return;
      }
    }
    this.fileName = fileToUpload.name;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('fileType', this.fileType.toString());

    this.http.post(`${this.fileApi}/upload`, formData, { reportProgress: true, observe: 'events' })
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
