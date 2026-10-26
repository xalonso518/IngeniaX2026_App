import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleLayoutComponent } from './single-layout.component';

describe('SingleLayoutComponent', () => {
  let component: SingleLayoutComponent;
  let fixture: ComponentFixture<SingleLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SingleLayoutComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SingleLayoutComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
