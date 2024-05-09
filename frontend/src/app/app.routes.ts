import { Routes } from '@angular/router';
import { TriggersComponent } from './triggers/triggers.component';
import { RandomTextComponent } from './random-text/random-text.component';
import { TwistersComponent } from './twisters/twisters.component';
import { ExercisesComponent } from './exercises/exercises.component';

export const routes: Routes = [
    { 
        path: '',
        component: RandomTextComponent
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
    }
];
