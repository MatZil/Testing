import { Question } from "./question";

export class Survey {
    id: number;
    title: string;
    authorId: number;
    anonymousAnswers: boolean;
    questions: Question[];
}
