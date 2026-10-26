import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainNavBarComponent } from './main-nav-bar.component';

describe('MainNavBarComponent', () => {
  let component: MainNavBarComponent;
  let fixture: ComponentFixture<MainNavBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainNavBarComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MainNavBarComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
