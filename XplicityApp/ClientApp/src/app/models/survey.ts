import { Question } from "./question";

export class Survey {
    id: number;
    title: string;
    authorId: number;
    guid: string;
    creationDate: Date;
    anonymousAnswers: boolean;
    questions: Question[];
}
