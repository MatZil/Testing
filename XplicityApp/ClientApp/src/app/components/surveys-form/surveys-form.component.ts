import { Component, OnInit, Input, SimpleChanges, OnChanges, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SurveyType } from '../../enums/surveyType';

@Component({
  selector: 'app-surveys-form',
  templateUrl: './surveys-form.component.html',
  styleUrls: ['./surveys-form.component.scss']
})
export class SurveysFormComponent implements OnInit, OnChanges {
  surveyForm: FormGroup;
  @Input() numberOfQuestions: number;
  hhhh: number[];
  aaa: number[][] = [,];
  surveyType: SurveyType[] = [];
  selectedSurveyType: SurveyType[] = [];
  typee = SurveyType;
  
  constructor(private formBuilder: FormBuilder, private changeDetection: ChangeDetectorRef) { }

  ngOnInit() {
    this.setDefaultValues();
    this.initializeFormGroup();
  }

  refreshAnswerType(type, i)
  {
    this.surveyType[i] = type;
  }

  fillArray(howMany)
  {
    this.hhhh = [];
    for(var i=1;i<=howMany;i++) {
      
    
    this.hhhh.push(i);
    }
  }

  fillArrayChoices(howMany, i)
  {
    this.aaa[i] = [];
    for(var j=1;j<=howMany;j++) {
      
    
    this.aaa[i].push(j);
    }
  }
  

  detectChanges()
  {
    this.changeDetection.detectChanges();
  }

  trackByFn(index, i) { return `${index}-${i.id}`; }

  setDefaultValues(): void {
    // this.newUser.clientId = 0;
    // this.newUser.parentalLeaveLimit = 0;
    // this.newUser.role = 'Employee';
    // this.newUser.daysOfVacation = 20;
    // this.newUser.isManualHolidaysInput = false;
    this.numberOfQuestions = 1;
  }

  initializeFormGroup(): void {
    this.surveyForm = this.formBuilder.group({
      numberOfQuestions: [this.numberOfQuestions]
    });
  }

  // ngOnChanges(changes: SimpleChanges) {
  //   console.log(changes['numberOfQuestions'].currentValue);
  // }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.numberOfQuestions) {
        console.log(changes.numberOfQuestions.currentValue);
    }
}


}
