import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { SurveyService } from '../../services/survey.service';
import { Survey } from '../../models/survey';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { QuestionType } from 'src/app/enums/questionType';

@Component({
  selector: 'app-surveys-answers-form',
  templateUrl: './surveys-answers-form.component.html',
  styleUrls: ['./surveys-answers-form.component.scss']
})
export class SurveysAnswersFormComponent implements OnInit {
  survey: Survey;
  currentUser: TableRowUserModel;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private surveyService: SurveyService) { }

  ngOnInit() {
    this.getSelectedSurveyByGuid(this.route.snapshot.paramMap.get('id'));
  }

  getSelectedSurveyByGuid(guid: string) {
    this.surveyService.getSurveyByGuid(guid).subscribe(selectedSurvey => {
      this.survey = selectedSurvey;
      console.log(this.survey);
    });
  }

  getSurveysAnonymical() {
    return this.survey.anonymousAnswers ? "" : "not ";
  }

  getQuestionType() {
    return QuestionType;
  }

  createSilderLabel(value: number) {
    return value;
  }

}
