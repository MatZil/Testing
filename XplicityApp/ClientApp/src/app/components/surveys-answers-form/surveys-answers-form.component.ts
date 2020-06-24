import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { SurveyService } from '../../services/survey.service';
import { Survey } from '../../models/survey';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { QuestionType } from 'src/app/enums/questionType';
import { Answer } from '../../models/answer';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { AnswerService } from 'src/app/services/answer.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-surveys-answers-form',
  templateUrl: './surveys-answers-form.component.html',
  styleUrls: ['./surveys-answers-form.component.scss']
})
export class SurveysAnswersFormComponent implements OnInit {
  survey: Survey;
  currentUser: TableRowUserModel;
  answers: Answer[] = [];
  answerFormGroup: FormGroup;
  allAnswers: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private surveyService: SurveyService,
    private formBuilder: FormBuilder,
    private answerService: AnswerService,
    private authenticationService: AuthenticationService
  ) {
    this.answerFormGroup = formBuilder.group({
      'answers': []
    });
  }

  ngOnInit() {
    this.getSelectedSurveyByGuid(this.route.snapshot.paramMap.get('id'));
  }

  getSelectedSurveyByGuid(guid: string) {
    this.surveyService.getSurveyByGuid(guid).subscribe(selectedSurvey => {
      this.survey = selectedSurvey;
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

  onSubmit() {
    this.allAnswers.push(this.answerFormGroup.value.answers);

    if (this.allAnswers.length == this.survey.questions.length) {
      for (var index = 0; index < this.survey.questions.length; index++) {
        let answer = new Answer();

        if (this.survey.anonymousAnswers) {
          answer.employeeId = undefined;
        } else {
          answer.employeeId = this.authenticationService.getUserId();
        }

        answer.questionId = this.survey.questions[index].id;
        console.log(this.survey.questions[index]);

        answer.answerText = this.allAnswers[index];


        this.answers.push(answer);
      }

      this.answerService.createAnswers(this.answers).subscribe();
    }

  }

}
