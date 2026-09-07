# Task 2: QA Strategy & Thinking

## Scenario
You've just joined a fintech startup as a QA Engineer. On day one you're handed a mobile trading app, for both iOS and Android, that's two weeks from its first public release. There's no existing test suite, no QA documentation, and the dev team has been shipping fast. **Real user funds are involved.**

---

## 1. Where Do You Start?

### Immediate Actions

**Priority 1: Risk Assessment & Critical Path Identification**
- **Understand the money flow**: Map out every user journey involving real funds - deposits, withdrawals, transfers, trades
- **Security audit baseline**: Password handling, authentication, authorization, data encryption, API security
- **Access production monitoring**: Get access to error logs, crash analytics, user feedback, support tickets

**Priority 2: Environment & Access Setup**
- Obtain access to all environments (dev, staging/test, uat)
- Get API documentation, architecture diagrams, database schemas
- Set up test devices (real iOS and Android devices)
- Configure testing tools

**Priority 3: Critical Functionality Smoke Test**
- Manually execute the most critical happy paths on both platforms(IOS and Android)
- Document any immediate blockers or critical bugs

### Strategic Planning

**Build the Test Foundation**
- Create a **risk matrix** (see RISK_MATRIX.md)
- Design a **release checklist** prioritizing financial transactions
- Map existing features into test categories (P0/P1/P2/P3)
- Start documenting test cases for critical paths

### Why This Approach?
With two weeks to release and real money at stake, I cannot afford to start by building a full automation suite. 
My first job is to **prevent catastrophic failure** - ensure we don't lose user funds, violate regulations, or ship security vulnerabilities. 
Test automation comes after I understand what's actually critical.

---

## 2. How Would You Approach Testing This App?

### Testing Strategy: Risk-Based & Layered

#### **Layer 1: Manual Exploratory Testing (Week 1)**
Focus on critical financial flows with real-world scenarios:

**Critical User Journeys**
1. **Account & Authentication**
   - Registration 
   - Login, logout, session management
   - Password reset, 2FA, biometric authentication
   - Account lockout after failed attempts

2. **Funds Management** (HIGHEST PRIORITY)
   - Verify balance calculations are accurate
   - Test transaction history and statements
   - Edge cases: minimum/maximum amounts, insufficient funds, failed transactions

3. **Security & Compliance**
   - SSL pinning, certificate validation
   - Session timeout and token expiry
   - API authorization (can User A access User B's data?)
   - SQL injection, XSS on inputs
   - Sensitive data exposure in logs/screenshots


#### **Layer 2: API Testing (Week 1-2)**
- Validate all financial transaction APIs independently
- Test authentication and authorization
- Check error handling and rollback mechanisms


#### **Layer 3: Automation (Post-Release, Ongoing)**
- Start with **API automation** (faster ROI than UI)
- Automate smoke tests for critical paths
- Build UI automation for regression (Appium/Detox/Maestro)
- Integrate into CI/CD pipeline


## 3. What Does QA Look Like Inside a Sprint, From Ticket Creation Through to Regression?

### Sprint Lifecycle QA Integration

#### **Pre-Sprint Planning**
- **Requirements Review**: Attend planning, review tickets for testability
- **Acceptance Criteria**: Ensure every ticket has clear, measurable acceptance criteria
- **Test Plan**: Identify test scenarios, edge cases, and automation candidates
- **Risk Assessment**: Flag high-risk features for extra scrutiny

#### **During Sprint**

**Day 1-2: Ticket Kickoff**
- Kickoff ticket , clarify edge cases, error handling, dependencies
- Identify testability concerns (logs, test data, feature flags)
- **Test Case Design**: Write test cases while devs code
- **Test Data Setup**: Prepare test accounts, mock data, API payloads
- **Environment Prep**: Ensure test environments are stable
- **Early Testing**: Test on feature branches as soon as PR is ready (shift-left)

 Active Testing**
- **Functional Testing**: Validate happy paths and edge cases
- **API Testing**: Validate backend changes independently
- **Cross-Platform**: Test on iOS and Android
- **Exploratory Testing**: Go beyond test cases, think like a malicious user

Bug Fixing & Retesting**
- Log bugs with clear repro steps, screenshots, logs
- Retest fixed bugs
- Verify no new bugs introduced

Pre-Release Validation**
- **Regression Suite**: Execute full regression (manual + automated)
- **Smoke Test**: Validate critical paths on staging
- **Release Notes**: Verify all changes are documented
- **Sign-Off**: QA approves or blocks release


## 4. What Does Your Ideal Regression Suite Look Like?

#### **Layer 1: Smoke Tests (Fast & Critical)**
**Execution Time**: 5-10 minutes  
**Frequency**: Every commit/PR, pre-deployment  
**Scope**: Critical happy paths only


#### **Layer 2: Functional Regression (Comprehensive)**
**Execution Time**: 1-2 hours  
**Frequency**: Daily, before each release  
**Scope**: All major features and workflows

**Coverage**:
- **Authentication**: Login, logout, 2FA, password reset
- **Funds**: Deposits, withdrawals, balance updates, transaction history
- **Profile**: Settings, preferences, notifications
- **Edge Cases**: Insufficient funds, network failures, expired sessions


## 5. What Would Keep You Up at Night About This App Specifically and Releasing to the Public?  

#### Financial Loss Bugs**
**Risk**: User deposits $1000, system shows $100. Withdrawal sends money twice.

**Mitigation**:
- Exhaustive testing of all money-in and money-out flows
- Database transaction integrity checks
- Reconciliation reports (expected vs. actual balances)
- Daily audit of all financial transactions
- Rollback mechanisms and transaction logs


#### Security Vulnerabilities** 
**Risk**: User A can access User B's account data. API endpoints aren't authenticated.

**Mitigation**:
- Penetration testing by security experts
- API authorization tests (can User A call User B's endpoints?)
- Encrypted storage, SSL pinning, secure token handling


####  Payment Gateway Failures** 
**Risk**: User deposits via credit card, payment succeeds at gateway but app never receives webhook. User charged but balance not updated.

**Mitigation**:
- Test webhook reliability and retry logic
- Manual reconciliation process for failed webhooks
- User can submit proof of payment for manual credit
- Logging and monitoring of all payment transactions
