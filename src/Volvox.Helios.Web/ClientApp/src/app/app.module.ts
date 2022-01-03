import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoggerModule } from '@volvox-ng/material';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        LoggerModule,
        MatProgressBarModule,
    ],
    providers: [],
    bootstrap: [ AppComponent ]
})
export class AppModule {
}
