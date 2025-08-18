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
    wpm?: number
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

export interface DayData {
  date: string;       // ISO формат даты, например "2025-08-16"
  done: boolean;      // выполнено ли задание
  rewarded: boolean;  //получена ли награда
  wordsRead: number;  // количество прочитанных слов
}

export interface CalendarData {
  year: number;        // текущий год
  month: number;       // месяц (0-11)
  days: DayData[];     // массив дней месяца
}
