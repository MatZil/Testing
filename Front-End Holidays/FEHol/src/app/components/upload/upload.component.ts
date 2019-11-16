import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {

  public progress: number;
  public selectedFileType: string;
  public fileName: string;
  private readonly snackbarConfiguration: MatSnackBarConfig = { duration: 5000 };
  constructor(private http: HttpClient, private snackBar: MatSnackBar) { }

  ngOnInit() {
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    this.fileName = fileToUpload.name;
    const formData = new FormData();
    console.log(this.selectedFileType);
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('fileType', this.selectedFileType);

    this.http.post('https://localhost:44374/api/file/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total);
        }
        else if (event.type === HttpEventType.Response) {
          this.snackBar.open('You have successfuly upload a file', 'OK', this.snackbarConfiguration);
        }
      });
  }
}
