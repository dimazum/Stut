import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ArticleData, CalendarData, Trigger, TriggerResult, TriggerTaskResult } from '../models/models';
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
  public startLesson(): Observable<any> {
    return this.httpClient.post<number>(`${this.baseUrl}/lesson/start`, null);
  }

  public finishLesson(id: number, words: number, wps: number) {
    const body = { id, words, wps };
    return this.httpClient.put(`${this.baseUrl}/lesson/finish`, body);
  }

  public rewardLesson(id: number, value: boolean) {
    const body = { id, value };
    return this.httpClient.put(`${this.baseUrl}/lesson/reward`, body);
  }
  public getRandomWord(w: string, count: number): Observable<string> {
    return this.httpClient.get(`${this.baseUrl}/trigger/randomWord/${w}/${count}`, { responseType: 'text' });
  }
}
