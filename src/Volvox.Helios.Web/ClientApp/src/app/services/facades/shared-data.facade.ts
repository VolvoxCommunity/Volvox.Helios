import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ISharedDataState } from '../../models/states/shared-data-state.model';

let _state: ISharedDataState = {
    isLoading: false,
};

@Injectable({
    providedIn: 'root'
})
export class SharedDataFacade {

    protected store$: BehaviorSubject<ISharedDataState> = new BehaviorSubject<ISharedDataState>(_state);

    constructor() {
    }

    public subState(): Observable<ISharedDataState> {
        return this.store$.asObservable();
    }

    private updateState(state: ISharedDataState): void {
        return this.store$.next(_state = state);
    }

}
