import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionType } from '../../enums/questionType';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SurveyService } from '../../services/survey.service';
import { Survey } from 'src/app/models/survey';
import { Question } from 'src/app/models/question';
import { Choice } from 'src/app/models/choice';

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
    let fg = this.fb.group({
        'questionText': [question ? question.questionText : ''],
        'type': [question ? question.type : QuestionType.Text_entry],
        'choices': this.fb.array([]),
    });
    (<FormArray>this.form.get('questions')).push(fg);
    let questionIndex = (<FormArray>this.form.get('questions')).length - 1;

    if (!question) {
        this.addChoice(questionIndex);
    }
    else {
        question.choice.forEach(choice => {
            this.addChoice(questionIndex, choice);
        });
    }
}

deleteQuestion(index: number) {
    (<FormArray>this.form.get('questions')).removeAt(index);
}

  addChoice(questionIndex: number, data?: any) {
     // console.log('questionIndex', questionIndex, '-------', 'data', data);
      let fg = this.fb.group({
          'choiceText': [data ? data : ''],
      });
      (<FormArray>(<FormGroup>(<FormArray>this.form.controls['questions'])
          .controls[questionIndex]).controls['choices']).push(fg);

  }

  deleteChoice(questionIndex: number, index: number) {
      //console.log('questionIndex', questionIndex, '-------', 'index', index);
      (<FormArray>(<FormGroup>(<FormArray>this.form.controls['questions'])
          .controls[questionIndex]).controls['choices']).removeAt(index);
  }

 

  onSubmit(formValue) {

    var survey = new Survey;
    survey.title = this.form.get('title').value;

    let isAnonymous = this.form.get('anonymousAnswers').value;
    if (isAnonymous)
      survey.anonymousAnswers = true;
    else survey.anonymousAnswers = false;

    survey.authorId = this.form.get('authorId').value;

    survey.questions = [];
    
    var questions = <FormArray>this.form.get('questions');

  for (let i = 0; i < questions.length; i++) {

     var q = new Question;
     q.questionText = questions.at(i).get('questionText').value;
     q.type = questions.at(i).get('type').value;
     q.choices = [];
      
      var choices = questions.at(i).get('choices') as FormArray;
      for (let j = 0; j < choices.length; j++) {
        var b = new Choice;
        b.choiceText = choices.at(j).get('choiceText').value;
        q.choices.push(b);
      }
      
      survey.questions.push(q);
  }
     
     let gh = this.surveyService.createNewSurvey(survey).subscribe();
  }



  ngOnDestroy(): void {
  }

isMultipleChoice(questionIndex: number, index: number) {
  let type = (<FormArray>(<FormGroup>(<FormArray>this.form.controls['questions']).controls[questionIndex]).controls['type']).value;

  if (type == QuestionType.Multiple_choice)
  return true;
}

  getQuestionColor(i) {
    if (i & 1) {
      return '#FFFFFF';
    } else {
      return '#F0F0F0';
    }
  }
}