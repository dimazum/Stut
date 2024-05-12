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

  export interface Trigger{
    value: string 
    difficulty: number
  }

  export interface TriggerResult{
    value: string 
    difficulty: number
    createdAt: Date
  }

  export interface TriggerTaskResult{
    description: string 
    values: Array<string>
    isStretch: boolean
  }