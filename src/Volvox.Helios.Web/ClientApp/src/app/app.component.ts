import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { CoreLoggerService, ILogEvent, ILoggerConfig, ILoggerDefaultConfig } from '@volvox-ng/core';
import { LoggerComponent, LoggerService } from '@volvox-ng/material';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { ISharedDataState } from './models/states/shared-data-state.model';
import { SharedDataFacade } from './services/facades/shared-data.facade';

@Component({
    selector: 'helios-root',
    templateUrl: './app.component.html',
    styleUrls: [ './app.component.scss' ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit {

    public sharedDataState$: Observable<ISharedDataState>;

    @ViewChild(LoggerComponent)
    public set logger(logger: LoggerComponent) {
        this.myLoggerService.logger(logger, this.defaultLoggerConfig);
    }

    private defaultLoggerConfig: ILoggerDefaultConfig;

    constructor(
        private readonly myCoreLoggerService: CoreLoggerService,
        private readonly mySharedDataFacade: SharedDataFacade,
        private readonly myLoggerService: LoggerService,
    ) {
    }

    public ngOnInit(): void {
        this.sharedDataState$ = this.mySharedDataFacade.subState();

        // Subscribe to ui logs
        this.handleLogs();
    }

    /**
     * Handle all logs that come from the components and the api
     * and display them to the user
     */
    private handleLogs(): void {
        this.defaultLoggerConfig = {
            debug: !environment.production,
            closeOnClick: false,
            showDismiss: true,
            showUser: true,
        };
        this.myCoreLoggerService.subUiLogs()
            .subscribe((data: ILogEvent): void => {
                if (data) {
                    this.myLoggerService.show({
                        title: data.title,
                        msg: data.msg,
                        ...data.config,
                        ...this.defaultLoggerConfig as ILoggerConfig,
                    }, data.type);
                }
            });
    }

}
