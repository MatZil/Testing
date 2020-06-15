import { Component, OnInit, Inject} from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { QuestionType } from '../../enums/questionType';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SurveyService } from '../../services/survey.service';
import { Survey } from 'src/app/models/survey';
import { Question } from 'src/app/models/question';
import { Choice } from 'src/app/models/choice';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-surveys-form',
  templateUrl: './surveys-form.component.html',
  styleUrls: ['./surveys-form.component.scss']
})
export class SurveysFormComponent implements OnInit {

  public form: FormGroup;

  constructor(
    private authenticationService: AuthenticationService,
    private surveyService: SurveyService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<SurveysFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SurveysFormComponent
  ) {
    this.form = fb.group({
      'title': [],
      'anonymousAnswers': [],
      'authorId': [],
      'questions': fb.array([])
    });
  }

  ngOnInit() {
    this.form.controls['authorId'].setValue(this.authenticationService.getUserId());
  }

  addQuestion(question?: any) {
    const fg = this.fb.group({
      'questionText': [question ? question.questionText : ''],
      'type': [question ? question.type : QuestionType.TextEntry],
      'choices': this.fb.array([]),
    });
    (<FormArray>this.form.get('questions')).push(fg);
    const questionIndex = (<FormArray>this.form.get('questions')).length - 1;

    if (!question) {
      this.addChoice(questionIndex);
    } else {
      question.choice.forEach(choice => {
        this.addChoice(questionIndex, choice);
      });
    }
  }

  deleteQuestion(index: number) {
    (<FormArray>this.form.get('questions')).removeAt(index);
  }

  addChoice(questionIndex: number, data?: any) {
    const fg = this.fb.group({
      'choiceText': [data ? data : ''],
    });
    (<FormArray>(<FormGroup>(<FormArray>this.form.controls['questions'])
      .controls[questionIndex]).controls['choices']).push(fg);
  }

  deleteChoice(questionIndex: number, index: number) {
    (<FormArray>(<FormGroup>(<FormArray>this.form.controls['questions'])
      .controls[questionIndex]).controls['choices']).removeAt(index);
  }

  onSubmit() {
    const survey = this.addFormArrayDataToSurvey();
    this.surveyService.createNewSurvey(survey).subscribe();
    this.dialogRef.close();
  }

  addFormArrayDataToSurvey() {
    const survey = new Survey;
    survey.title = this.form.get('title').value;

    const isAnonymous = this.form.get('anonymousAnswers').value;
    if (isAnonymous) {
      survey.anonymousAnswers = true;
    } else {
      survey.anonymousAnswers = false;
    }

    survey.authorId = this.form.get('authorId').value;

    survey.questions = [];

    const questions = <FormArray>this.form.get('questions');

    for (let i = 0; i < questions.length; i++) {

      const question = new Question;
      question.questionText = questions.at(i).get('questionText').value;
      question.type = questions.at(i).get('type').value;
      question.choices = [];

      const choices = questions.at(i).get('choices') as FormArray;
      for (let j = 0; j < choices.length; j++) {
        const choice = new Choice;
        choice .choiceText = choices.at(j).get('choiceText').value;
        question.choices.push(choice);
      }

      survey.questions.push(question);
    }

    return survey;
  }

  isMultipleChoice(questionIndex: number, index: number) {
    const type = (<FormArray>(<FormGroup>(<FormArray>this.form.controls['questions']).controls[questionIndex]).controls['type']).value;

    if (type === QuestionType.MultipleChoice) {
        return true;
      }
  }
}
