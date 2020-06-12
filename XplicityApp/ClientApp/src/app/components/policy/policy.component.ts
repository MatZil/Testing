import { Component, OnInit, HostListener } from '@angular/core';
import { FilesService } from '../../services/files.service';
import { UrlService } from 'src/app/services/url.service';

@Component({
  selector: 'app-policy',
  templateUrl: './policy.component.html',
  styleUrls: ['./policy.component.scss']
})
export class PolicyComponent implements OnInit {
  policyPath: string;
  showPolicy = false;

  constructor(
    private fileService: FilesService,
    private urlService: UrlService) { }

  ngOnInit() {
  }

  onButtonClick() {
    this.fileService.getPolicyUrl().subscribe(url => {
      this.policyPath = url;
      this.showPolicy = true;
    }
    );
  }

  @HostListener('document:click', ['$event'])
  exitPdf() {
    this.showPolicy = false;
  }
}
