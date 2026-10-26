import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChipStatusComponent } from './chip-status.component';

describe('ChipStatusComponent', () => {
  let component: ChipStatusComponent;
  let fixture: ComponentFixture<ChipStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChipStatusComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ChipStatusComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
