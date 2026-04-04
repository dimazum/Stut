import { Time } from "@angular/common";
import { DailyLessonStatus } from "./enums";

export interface ArticleData {
  id: string;
  title: string;
  topic: string;
  source: string;
  content: string;
  locale: string;
  ageGroup: number;
  createdAt: Date;
  categories: string[];
  initCategory: string;
}

export class RecognitionData {
  public text?: string;
  public wordCount: number = 0;
  public wpm?: number;
}

export interface Trigger {
  value: string;
  difficulty: number;
}

export interface TriggerResult {
  value: string;
  difficulty: number;
  createdAt: Date;
}

export interface TriggerTaskResult {
  description: string;
  values: Array<string>;
  isStretch: boolean;
}

export interface DayData {
  lessonId: number;
  date: string; // ISO формат даты, например "2025-08-16"
  done: boolean; // выполнено ли задание
  rewarded: boolean; //получена ли награда
  wordsRead: number; // количество прочитанных слов
}

export interface CalendarData {
  year: number; // текущий год
  month: number; // месяц (0-11)
  days: DayData[]; // массив дней месяца
}

export interface DayLessonEnabledDto {
  enabled: boolean;
  secondsRemaining: number;
}

export interface DayLessonDto {
  id: number;
  userId: string;
  status: DailyLessonStatus;
  startTime: Date;
  wordsSpoken: number;
  wps: number;
  leftInSec: number;
}

export interface AudioFile {
  fileName: string;
  fullPath: string;
  uploadedAt: string;
}

export interface VoiceAnalysisResult {
  duration: number;
  pitchMean?: number;
  pitchStd?: number;
  pitchMin?: number;
  pitchMax?: number;
  volumeMeanDb: number;
  volumeStdDb: number;
  volumePeakDb: number;
  speechRate: number;
  pauseRatio: number;
  jitter?: number;
  shimmer?: number;
  mfccMean: number[];
  recordedAt: string; // дата записи
}

export interface VoiceAnalysisUpdateDto {
  sessionId: string;
  jitter: number;
  shimmer: number;
  wpm: number;
  wordsSpoken: number;
}

export interface Histogram {
  name: string;
  chars: CharItem[];
}

export interface CharItem {
  id: number;
  char: string;
  order: number;
  air: number;
}

export interface ConfirmEmailDto {
  userId: string | null;
  token: string | null;
}

export interface SendResetPasswordDto {
  email: string | null;
}

export interface ResetPasswordDto {
  userId: string | null;
  token: string | null;
  password: string | null;
}
