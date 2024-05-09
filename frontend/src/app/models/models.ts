export interface ArticleData{
    id: string
    title: string,
    content: string 
    locale: string,
    ageGroup: number,
    createdAt: Date
  }

  export class RecognitionData{
    text?: string 
    wordCount?: number
  }