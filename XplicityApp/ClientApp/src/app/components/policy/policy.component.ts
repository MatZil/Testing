import { Component, OnInit, Inject } from '@angular/core';
import { FilesService } from '../../services/files.service';

@Component({
  selector: 'app-policy',
  templateUrl: './policy.component.html',
  styleUrls: ['./policy.component.scss']
})
export class PolicyComponent implements OnInit {
  pageNumber = 1;
  policyPath: string;
  showPolicy = false;
  baseApplicationUrl: string;
  constructor(
    private fileService: FilesService,
    @Inject('BASE_URL') baseUrl: string) {
      this.baseApplicationUrl = baseUrl;
    }

  ngOnInit() {
  }

  onButtonClick() {
    this.fileService.getPolicyRelativePath().subscribe(relativePath => {
        this.policyPath = this.baseApplicationUrl + relativePath;
      }
    );
    this.showPolicy = true;
    const img: any = document.querySelector(this.policyPath);
    if (typeof (FileReader) !== 'undefined') {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        this.policyPath = e.target.result;
      };

      reader.readAsArrayBuffer(img.files[0]);
    }
  }

  exitPdf() {
    this.showPolicy = false;
  }

}
