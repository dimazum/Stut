import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { ArticleData, AudioFile, CalendarData, DayLessonDto, Trigger, TriggerResult, TriggerTaskResult, VoiceAnalysisResult } from '../models/models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  public baseUrl = environment.apiUrl;

  public constructor(private httpClient: HttpClient) {}

  public getTwisters(): Observable<Array<Array<string>>> {
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/twisters`);
  }

  public getExercises(): Observable<Array<Array<string>>> {
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/stretching`);
  }

  public getRandomArticle(): Observable<ArticleData> {
    return this.httpClient.get<ArticleData>(`${this.baseUrl}/article/random`);
  }

  public createTrigger(trigger: Trigger): Observable<TriggerResult> {
    return this.httpClient.post<TriggerResult>(`${this.baseUrl}/trigger/create`, trigger);
  }

  public getTriggers(): Observable<Array<TriggerResult>> {
    return this.httpClient.get<Array<TriggerResult>>(`${this.baseUrl}/trigger`);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public deleteTrigger(triggerValue: string): Observable<any> {
    return this.httpClient.delete(`${this.baseUrl}/trigger/${triggerValue}`);
  }

  public getTriggerTasks(triggerValue: string): Observable<Array<TriggerTaskResult>> {
    return this.httpClient.get<Array<TriggerTaskResult>>(`${this.baseUrl}/trigger/triggertasks/${triggerValue}`);
  }

  public getCalendar(): Observable<CalendarData> {
    return this.httpClient.get<CalendarData>(`${this.baseUrl}/calendar/get`);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public startLesson(): Observable<DayLessonDto> {
  return this.httpClient
    .post<DayLessonDto>(`${this.baseUrl}/lesson/start`, null);
  }

  public getDailyLesson(): Observable<DayLessonDto> {
    return this.httpClient
      .get<DayLessonDto>(`${this.baseUrl}/lesson/daily`);
  }

  public pauseLesson(id: number, words: number, wps: number): Observable<DayLessonDto> {
    const body = { id, words, wps };
    return this.httpClient
      .put<DayLessonDto>(`${this.baseUrl}/lesson/pause`, body);
  }

  public finishLesson(id: number, words: number, wps: number): Observable<DayLessonDto> {
    const body = { id, words, wps };
    return this.httpClient
      .put<DayLessonDto>(`${this.baseUrl}/lesson/finish`, body);
  }

  public rewardLesson(id: number, value: boolean) {
    const body = { id, value };
    return this.httpClient.put(`${this.baseUrl}/lesson/reward`, body);
  }
  public getRandomWord(w: string, count: number): Observable<string> {
    return this.httpClient.get(`${this.baseUrl}/trigger/randomWord/${w}/${count}`, { responseType: 'text' });
  }

  public getCurrentCommitHash(): Observable<string> {
    return this.httpClient.get(`${this.baseUrl}/versioning/hash`, { responseType: 'text' });
  }

//AudioService
  public uploadRecording(blob: Blob) {
    const formData = new FormData();
    formData.append('file', blob, 'recording.webm');

    return this.httpClient.post<FormData>(`${this.baseUrl}/voiceAnalysis/upload`, formData);
  }

  public getList(): Observable<AudioFile[]> {
    return this.httpClient.get<AudioFile[]>(`${this.baseUrl}/audio/list`);
  }

  public getFileUrl(fileName: string): string {
    return `${this.baseUrl}/audio/file/${fileName}`;
  }

  public deleteFile(fileName: string) {
  return this.httpClient.delete(`/api/audio/${fileName}`);
  }

  public getVoiceAnalysisResults() {
  return this.httpClient.get<VoiceAnalysisResult[]>(`/api/voiceAnalysis/last`);
  }
}
