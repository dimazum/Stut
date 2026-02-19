import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, shareReplay, tap } from 'rxjs';
import { ArticleData, AudioFile, CalendarData, DayLessonDto, Histogram, Trigger, TriggerResult, TriggerTaskResult, VoiceAnalysisResult } from '../models/models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  public baseUrl = environment.baseUrl + '/api';

  private rewardPoints$?: Observable<number>;

  public constructor(private httpClient: HttpClient) {}

  public getTwisters(): Observable<Array<Array<string>>> {
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/twisters`);
  }

  public getExercises(): Observable<Array<Array<string>>> {
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/stretching`);
  }

  public getRandomArticle(category: string): Observable<ArticleData> {
    return this.httpClient.get<ArticleData>(`${this.baseUrl}/article/random?category=${category}`);
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

  getTriggerExercises(): Observable<{ [key: string]: string[] }> {
    return this.httpClient.get<{ [key: string]: string[] }>(`${this.baseUrl}/trigger/generate`);
  }

  public getCalendar(year: number, month: number): Observable<CalendarData> {
    return this.httpClient.get<CalendarData>(`${this.baseUrl}/calendar?year=${year}&month=${month}`);
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


  getRewardPoints(): Observable<number> {
    if (!this.rewardPoints$) {
      this.rewardPoints$ = this.httpClient
        .get<number>(`${this.baseUrl}/lesson/rewardPoints`)
        .pipe(
          shareReplay(1)
        );
    }

    return this.rewardPoints$;
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

  public getOrCreateHistogram(name: string, initText: string, saveToDb: boolean): Observable<Histogram>{
    return this.httpClient.post<Histogram>(`${this.baseUrl}/histogram/getOrCreateHistogram`, {name, initText, saveToDb});
  }

  public saveHistogram(histogram: Histogram): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/histogram/save`, histogram);
}

  public addHistogramColumn(name: string, order: number): Observable<Histogram>{
    return this.httpClient.get<Histogram>(`${this.baseUrl}/histogram/addcolumn?name=${name}&order=${order}`);
  }

  public remoHistogramColumn(name: string, order: number): Observable<Histogram>{
    return this.httpClient.get<Histogram>(`${this.baseUrl}/histogram/removecolumn?name=${name}&order=${order}`);
  }

}
