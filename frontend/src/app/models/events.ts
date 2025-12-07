import { Subject } from 'rxjs';
import { DayLessonEnabledDto } from './models';

export const startLessonSubject = new Subject<DayLessonEnabledDto>();
