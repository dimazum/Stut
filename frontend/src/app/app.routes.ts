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
import { LarynxComponent } from './larynx/larynx.component';

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
            { path: '', redirectTo: 'larynx', pathMatch: 'full' } // редирект на "Основы"
            ]
    } 
];