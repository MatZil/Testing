import { QuestionType } from '../enums/questionType';
import { Choice } from './choice';

export class Question {
    id: number;
    surveyId: number;
    type: QuestionType;
    questionText: string;
    choices: Choice[];
}
