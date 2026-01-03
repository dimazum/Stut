import { Subject } from 'rxjs';
import { DayLessonEnabledDto } from './models';

export const startLessonSubject = new Subject<DayLessonEnabledDto>();
export const notLoggedInSubject = new Subject();
