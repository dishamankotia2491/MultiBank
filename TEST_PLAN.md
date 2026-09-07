# Test Plan: MultiBank Trading Platform

## Document Control
- **Project**: MultiBank Web Trading Platform QA Automation
- **Version**: 1.0
- **Author**: QA Automation Engineer


## 1. Executive Summary

### Objective
Validate the core functionality, performance, security, and user experience of the MultiBank trading platform (https://trade.mb.io/) 
through comprehensive automated and manual testing.

### Scope
- Web UI testing across multiple browsers and devices
- Critical user journeys (navigation, trading, content)
- Negative and edge case scenarios
- Cross-browser compatibility
- Responsive design validation

### Out of Scope
- Backend/database testing
- Performance/load testing 
- Security penetration testing
- Mobile native app testing
- Payment gateway integration testing


## 2. Test Strategy

### Testing Approach
**Risk-Based Testing**: Prioritize test scenarios based on business impact and user frequency.

For this project, focus is on **E2E UI Tests** with **Page Object Model** architecture.

### Testing Types

| Type | Priority | Coverage |
|------|----------|----------|
| Functional | P0 | Navigation, Trading, Content display |
| Usability | P1 | User flows, error messages, intuitive design |
| Compatibility | P1 | Cross-browser (Chrome, Firefox, Safari/WebKit) |
| Responsive | P1 | Desktop, tablet, mobile viewports |
| Negative | P1 | Invalid routes, broken links, timeouts |


## 3. Test Scope

### 3.1 In Scope

#### Navigation & Layout
- ✅ Top navigation renders with expected items
- ✅ Navigation links route to correct destinations
- ✅ Responsive behavior at desktop viewports (1920x1080, 1366x768, 1440x900)
- ✅ Responsive behavior at mobile viewports (375x667, 390x844, 360x800)
- ✅ Navigation accessibility (href attributes, semantic HTML)

#### Trading Functionality
- ✅ Spot trading section renders correctly
- ✅ Trading pairs display with data fields (Symbol, Price, Change)
- ✅ Trading pairs grouped into categories (Forex, Crypto, Indices, etc.)
- ✅ Trading pair data accuracy
- ✅ Page load performance for trading section

#### Content & Links
- ✅ Marketing banners visible in correct page region
- ✅ App Store download link resolves correctly
- ✅ Google Play download link resolves correctly
- ✅ About Us > Why MultiBank page renders with expected components


#### Negative / Edge Cases
- ✅ Invalid route handling (404 errors)
- ✅ Broken link detection across navigation
- ✅ Viewport regression at mobile breakpoints
- ✅ Content loading timeout handling


### 3.2 Out of Scope
- User authentication (login/logout) - not required per requirements
- Account creation and management
- Real trading transactions
- Backend API direct testing 
- Database validation
- Email notifications


## 4. Test Environment

### Browsers
- **Chromium**: Latest stable (121.0+)
- **Firefox**: Latest stable (122.0+)

### Viewports

**Desktop**:
- 1920x1080 (Full HD)
- 1366x768 (Standard Laptop)
- 1440x900 (MacBook Air)

**Mobile**:
- 375x667 (iPhone X)
- 390x844 (iPhone 12 Pro)
- 360x800 (Samsung Galaxy S20)

**Tablet**:
- 768x1024 (iPad)


### Environment URLs
- **Production**: https://trade.mb.io/
- **No login required**: All tests run on public pages



## 5. Test Execution

### Execution Schedule

| Phase | Activity | Duration |
|-------|----------|----------|
| **Phase 1** | Test framework setup | 2 hours |
| **Phase 2** | Page Object Model implementation | 2 hours |
| **Phase 3** | Test case development | 3 hours |
| **Phase 4** | Test execution & debugging | 2 hours |
| **Phase 5** | Cross-browser testing | 1 hour |
| **Phase 6** | Documentation & reporting | 1 hour |

**Total Estimated Time**: 11 hours (within 4-6 hour estimation with parallel work)

### Entry Criteria
- ✅ Test environment accessible (https://trade.mb.io/)
- ✅ Test automation framework configured (Playwright + C#)
- ✅ Page Object Model classes implemented
- ✅ Test data prepared

### Exit Criteria
- ✅ All P0 tests passed
- ✅ 90% of P1 tests passed
- ✅ Cross-browser testing completed
- ✅ Test report generated
- ✅ Screenshots captured for evidence
- ✅ Known issues documented


## 6. Test Deliverables

### Test Reports
1. ✅ Playwright HTML Report (auto-generated)
2. ✅ Console output with test results
3. ✅ Screenshots (stored in `/screenshots` directory)
4. ✅ Cross-browser test matrix


## 7. Risks & Assumptions

### Risks

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Application unavailable | High | Low | Use alternative URL, test on staging |
| Page structure changes | Medium | Medium | Use flexible selectors, update POM |
| Network instability | Medium | Medium | Implement retry logic, increase timeouts |
| Flaky tests | Medium | High | Use explicit waits, isolate tests |
| Browser compatibility issues | Medium | Low | Test on multiple browsers, update drivers |

### Assumptions
1. Target application (https://trade.mb.io/) is stable and accessible
2. No authentication required for test scenarios
3. Page structure remains relatively stable during testing
4. Standard desktop (1920x1080) and mobile (375x667) viewports
5. Latest browser versions available for testing


## 8. Test Metrics

### Coverage Metrics
- **Test Cases**: 14 automated test cases
- **Page Objects**: 3 page classes
- **Test Categories**: 3 (Navigation, Trading, Content, Edge Cases)
- **Browsers**: 2 (Chromium, Firefox)
- **Viewports**:  (Desktop, Mobile, Tablet)

### Success Criteria
- ✅ **Pass Rate**: ≥ 95% of P0 tests pass
- ✅ **Execution Time**: ≤ 60 seconds for full suite
- ✅ **Cross-Browser**: All tests pass on Chromium, Firefox, WebKit
- ✅ **Code Quality**: Clean, maintainable, well-documented code
- ✅ **Assertions**: Meaningful, resilient, descriptive assertions

---

## 10. Approval

| Role | Name | Signature | Date |
|------|------|-----------|------|
| QA Lead | | | |
| Dev Lead | | | |
| Product Owner | | | |



## 11. Revision History

| Version | Date | Author | Changes |
|---------|------|--------- |---------|
| 1.0 | 2026-09-07 | QA Engineer | Initial test plan creation |


## Appendix A: Test Execution Commands

```bash
# Run all tests
dotnet test

# Run specific category
dotnet test --filter "Category=Navigation"
dotnet test --filter "Category=Content"
dotnet test --filter "Category=EdgeCases"

# Run specific test
dotnet test --filter "FullyQualifiedName~TopNavigationRendersWithAllExpectedItems"

# Run with specific browser
dotnet test -- Playwright.BrowserName=chromium
dotnet test -- Playwright.BrowserName=firefox

# Run in headed mode (see browser)
dotnet test -- Playwright.LaunchOptions.Headless=false
```

---

## Appendix B: Useful Links
- **Target Application**: https://trade.mb.io/
- **Playwright Documentation**: https://playwright.dev/dotnet/
- **NUnit Documentation**: https://docs.nunit.org/
