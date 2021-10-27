import { TestBed } from '@angular/core/testing';

import { SharedDataFacade } from './shared-data.facade';

describe('SharedDataFacade', () => {
    let service: SharedDataFacade;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(SharedDataFacade);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
