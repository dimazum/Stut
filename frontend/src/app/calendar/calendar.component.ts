import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BackendService } from '../services/backend.service';
import { CalendarData, DayData } from '../models/models';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';

interface DayCell {
  lessonId: number;
  date: Date | null;
  done: boolean;
  rewarded: boolean;
  wordsRead: number;
  isToday: boolean;
}

@Component({
  selector: 'stu-calendar',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css'],
})
export class CalendarComponent implements OnInit, OnDestroy {
  public userinfo$ = this.authServeice.userinfo$;
  public subscription!: Subscription;

  public msg: string  = 'Пройдите регистрацию чтобы увидеть календарь'
  public loggedIn: boolean = false;

  public calendarGrid: DayCell[][] = [];
  public today = new Date();
  public currentMonthDate = new Date();
  public currentMonthName = '';
  public currentYear = 0;

  constructor(
    private backendService: BackendService,
    private authServeice: AuthService
  ) {}

  ngOnInit(): void {
  this.subscription = this.authServeice.userinfo$.subscribe(x => 
      {
        if(x?.logged_in){
          this.loadCalendar();
        }
      }
    )
  }

  // ------------------ Загрузка календаря ------------------
  public loadCalendar(): void {
    const year = this.currentMonthDate.getFullYear();
    const month = this.currentMonthDate.getMonth();

    this.backendService.getCalendar(year, month).subscribe({
      next: (data: CalendarData) => this.buildCalendar(data)
    });
  }

  private formatDateLocal(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private isSameDay(d1: Date, d2: Date): boolean {
    return d1.getFullYear() === d2.getFullYear() &&
           d1.getMonth() === d2.getMonth() &&
           d1.getDate() === d2.getDate();
  }

  // ------------------ Построение сетки ------------------
  public buildCalendar(data: CalendarData): void {
    this.currentYear = data.year;
    this.currentMonthName = new Date(data.year, data.month).toLocaleString('default', { month: 'long' });

    const firstDay = new Date(data.year, data.month, 1);
    const lastDay = new Date(data.year, data.month + 1, 0);

    const calendar: DayCell[][] = [];
    let week: DayCell[] = [];

    // Сдвиг, чтобы неделя начиналась с понедельника
    const jsDay = firstDay.getDay();
    const startOffset = jsDay === 0 ? 6 : jsDay - 1;

    for (let i = 0; i < startOffset; i++) {
      week.push({ lessonId: 0, date: null, done: false, rewarded: false, wordsRead: 0, isToday: false });
    }

    for (let day = 1; day <= lastDay.getDate(); day++) {
      const dateObj = new Date(data.year, data.month, day);
      const dateStr = this.formatDateLocal(dateObj);

      const dayData: DayData | undefined = data.days.find(d => d.date === dateStr);

      week.push({
        lessonId: dayData?.lessonId ?? 0,
        date: dateObj,
        done: dayData?.done ?? false,
        rewarded: dayData?.rewarded ?? false,
        wordsRead: dayData?.wordsRead ?? 0,
        isToday: this.isSameDay(dateObj, this.today)
      });

      if (week.length === 7) {
        calendar.push(week);
        week = [];
      }
    }

    // Дополняем последнюю неделю пустыми днями
    while (week.length < 7 && week.length > 0) {
      week.push({ lessonId: 0, date: null, done: false, rewarded: false, wordsRead: 0, isToday: false });
    }
    if (week.length) calendar.push(week);

    this.calendarGrid = calendar;
  }

  // ------------------ Навигация по месяцам ------------------
  public prevMonth(): void {
    this.currentMonthDate = new Date(this.currentMonthDate.getFullYear(), this.currentMonthDate.getMonth() - 1, 1);
    this.loadCalendar();
  }

  public nextMonth(): void {
    this.currentMonthDate = new Date(this.currentMonthDate.getFullYear(), this.currentMonthDate.getMonth() + 1, 1);
    this.loadCalendar();
  }

  // ------------------ Обработка изменений ------------------
  public onRewardChange(day: DayCell, event: Event): void {
    const input = event.target as HTMLInputElement;
    const checked = input.checked;
    this.backendService.rewardLesson(day.lessonId, checked).subscribe();
  }

    ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
