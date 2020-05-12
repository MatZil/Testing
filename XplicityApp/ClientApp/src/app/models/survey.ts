import { SurveyType } from '../enums/surveyType';

export class Survey {
    id: number;
    title: string;
    authorId: number;
    guid: string;
    type: SurveyType;
}
