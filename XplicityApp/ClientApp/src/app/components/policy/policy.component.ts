import { Component, OnInit, Inject, ElementRef, HostListener } from '@angular/core';
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
    private elementRef: ElementRef,
    @Inject('BASE_URL') baseUrl: string) {
      this.baseApplicationUrl = baseUrl;
    }

  ngOnInit() {
  }

  onButtonClick() {
    this.fileService.getPolicyRelativePath().subscribe(relativePath => {
        this.policyPath = this.baseApplicationUrl + relativePath;
        this.showPolicy = true;
      }
    );
  }

  @HostListener('document:click', ['$event'])
  exitPdf(event: any) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.showPolicy = false;
    }
  }

}
