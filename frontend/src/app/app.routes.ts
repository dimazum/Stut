import { Routes } from '@angular/router';
import { TriggersComponent } from './triggers/triggers.component';
import { RandomTextComponent } from './random-text/random-text.component';
import { TwistersComponent } from './twisters/twisters.component';
import { ExercisesComponent } from './exercises/exercises.component';
import { DiaphragmComponent } from './diaphragm/diaphragm.component';
import { CalendarPageComponent } from './calendar-page/calendar-page.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { LessonsComponent } from './lessons/lessons.component';
import { LarynxComponent } from './lessons/larynx/larynx.component';
import { Lesson1Component } from './lessons/lesson1/lesson1.component';
import { Lesson2Component } from './lessons/lesson2/lesson2.component';
import { Lesson3Component } from './lessons/lesson3/lesson3.component';
import { Lesson4Component } from './lessons/lesson4/lesson4.component';
import { Lesson5Component } from './lessons/lesson5/lesson5.component';
import { Lesson6Component } from './lessons/lesson6/lesson6.component';
import { Lesson7Component } from './lessons/lesson7/lesson7.component';

export const routes: Routes = [
    { 
        path: '',
        component: RandomTextComponent
    },
        { 
        path: 'register',
        component: RegisterComponent
    },
    { 
        path: 'login',
        component: LoginComponent
    },
    { 
        path: 'triggers',
        component: TriggersComponent
    },
    { 
        path: 'twisters',
        component: TwistersComponent
    },
    { 
        path: 'exercises',
        component: ExercisesComponent
    },
    { 
        path: 'diaphragm',
        component: DiaphragmComponent
    },
    { 
        path: 'calendar-page',
        component: CalendarPageComponent
    },
    { 
        path: 'lessons',
        component: LessonsComponent,
        children: [
            { path: 'larynx', component: LarynxComponent },
            { path: 'lesson1', component: Lesson1Component },
            { path: 'lesson2', component: Lesson2Component },
            { path: 'lesson3', component: Lesson3Component },
            { path: 'lesson4', component: Lesson4Component },
            { path: 'lesson5', component: Lesson5Component },
            { path: 'lesson6', component: Lesson6Component },
            { path: 'lesson7', component: Lesson7Component },
            { path: '', redirectTo: 'larynx', pathMatch: 'full' } // редирект на "Основы"
            ]
    } 
];