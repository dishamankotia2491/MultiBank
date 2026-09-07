# Risk Assessment Matrix

## Mobile Fintech Trading Application - Risk Analysis

**Project**: Mobile Trading App (iOS & Android)  
**Assessment Date**: September 6, 2026  
**Assessor**: QA Engineering Team  
**Status**: Pre-Release (2 weeks to public launch)  

---

## Risk Scoring

### Impact Scale
- **5 - Critical**: Catastrophic financial loss, security breach, regulatory violation, app unusable
- **4 - High**: Major feature broken, significant user impact, potential financial loss
- **3 - Medium**: Important feature degraded, workaround exists, user frustration
- **2 - Low**: Minor feature issue, minimal user impact, cosmetic
- **1 - Minimal**: Edge case, rare occurrence, negligible impact

### Probability Scale
- **5 - Very High (>60%)**: Almost certain to occur
- **4 - High (40-60%)**: Likely to occur
- **3 - Medium (20-40%)**: Possible to occur
- **2 - Low (5-20%)**: Unlikely but possible
- **1 - Very Low (<5%)**: Rare occurrence

### Risk Level Calculation
**Risk Score = Impact × Probability**

| Score | Risk Level | Action Required |
|-------|------------|-----------------|
| 20-25 | 🔴 **CRITICAL** | Immediate action, release blocker |
| 12-19 | 🟠 **HIGH** | Urgent mitigation, review before release |
| 6-11 | 🟡 **MEDIUM** | Plan mitigation, monitor closely |
| 3-5 | 🟢 **LOW** | Accept or minor mitigation |
| 1-2 | ⚪ **MINIMAL** | Accept risk |

---

## Risk Matrix Overview

| Risk ID | Risk Description | Impact | Probability | Score | Level | Status |
|---------|------------------|--------|-------------|-------|-------|--------|
| R-001 | Financial loss bugs (incorrect balance, double withdrawal) | 5 | 4 | 20 | 🔴 CRITICAL | Open |
| R-002 | Security breach (unauthorized access, data leak) | 5 | 3 | 15 | 🟠 HIGH | Open |
| R-003 | Payment gateway failures (webhook loss, duplicate charges) | 5 | 3 | 15 | 🟠 HIGH | Open |
| R-004 | Race conditions in trading (double orders, wrong prices) | 5 | 3 | 15 | 🟠 HIGH | Open |
| R-005 | Compliance violations (KYC bypass, AML failure) | 5 | 2 | 10 | 🟡 MEDIUM | Open |
| R-006 | Catastrophic crashes (app won't launch, trading broken) | 4 | 4 | 16 | 🟠 HIGH | Open |
| R-007 | Performance degradation (slow load times, timeouts) | 4 | 3 | 12 | 🟠 HIGH | Open |
| R-008 | Data sync issues (balance mismatch across devices) | 4 | 3 | 12 | 🟠 HIGH | Open |
| R-009 | Third-party API failures (price feed down, payment gateway offline) | 4 | 3 | 12 | 🟠 HIGH | Open |
| R-010 | No rollback/hotfix plan (critical bug in production, can't revert) | 4 | 3 | 12 | 🟠 HIGH | Open |
| R-011 | Device compatibility issues (crashes on specific Android devices) | 3 | 4 | 12 | 🟠 HIGH | Open |
| R-012 | Network failures (poor handling of offline/slow network) | 3 | 4 | 12 | 🟠 HIGH | Open |
| R-013 | Session management issues (premature logout, token expiry) | 3 | 3 | 9 | 🟡 MEDIUM | Open |
| R-014 | Push notification failures (user misses critical alerts) | 3 | 3 | 9 | 🟡 MEDIUM | Open |
| R-015 | App store rejection (policy violations, metadata issues) | 3 | 3 | 9 | 🟡 MEDIUM | Open |
| R-016 | Localization issues (wrong currency, date format) | 3 | 2 | 6 | 🟡 MEDIUM | Open |
| R-017 | Accessibility barriers (screen reader issues) | 2 | 3 | 6 | 🟡 MEDIUM | Open |
| R-018 | Support unpreparedness (team doesn't know how to help users) | 3 | 3 | 9 | 🟡 MEDIUM | Open |


## Detailed Risk Analysis

### 🔴 CRITICAL RISKS

#### **R-001: Financial Loss Bugs**
**Impact**: 5 | **Probability**: 4 | **Score**: 20 | **Level**: 🔴 CRITICAL

**Description**:  
User deposits $1000 but balance shows $100. User withdraws $500, funds sent twice. Trading order executes but balance not debited. Database transaction failures lead to inconsistent balances.

**Business Impact**:
- Direct financial loss to company or users
- Lawsuits, chargebacks, fraud claims
- Regulatory fines and audits
- Reputation damage, loss of user trust

**Technical Causes**:
- Database transaction isolation issues
- Float/decimal precision errors in calculations
- Failed API calls not rolled back
- Webhook failures (payment succeeded but app didn't update)

**Mitigation Plan**:
1. **Testing**: Exhaustive testing of all deposit/withdrawal/trade flows
2. **Reconciliation**: Daily automated reconciliation (expected vs. actual balances)
3. **Database**: Use database transactions with ACID guarantees
4. **Idempotency**: All financial APIs must be idempotent (retry-safe)
5. **Logging**: Comprehensive logging of every financial transaction
6. **Monitoring**: Real-time alerts for balance discrepancies
7. **Manual Review**: Finance team reviews all transactions daily for first month


### 🟠 HIGH RISKS

#### **R-002: Security Breach**
**Impact**: 5 | **Probability**: 3 | **Score**: 15 | **Level**: 🟠 HIGH

**Description**:  
User A can view/modify User B's account. Session tokens don't expire. API endpoints not authenticated. SQL injection in search fields. Passwords stored in plain text. SSL not pinned, man-in-the-middle attack possible.

**Business Impact**:
- Data breach, user funds stolen
- Legal liability, class-action lawsuits
- Complete loss of user trust
- Regulatory license revocation

**Technical Causes**:
- Missing authorization checks on API endpoints
- Weak password policies
- Session tokens never expire
- SQL injection vulnerabilities
- Insecure data storage


**Mitigation Plan**:
1. **Penetration Testing**: Third-party security audit (mandatory)
2. **Authorization**: Every API endpoint checks user owns resource
3. **Session Management**: Tokens expire after 24 hours, refresh mechanism
4. **Input Validation**: All inputs sanitized (SQL injection, XSS)
5. **Encryption**: Data encrypted at rest (AES-256), in transit (TLS 1.3)


#### **R-003: Payment Gateway Failures**
**Impact**: 5 | **Probability**: 3 | **Score**: 15 | **Level**: 🟠 HIGH

**Description**:  
User deposits via credit card, payment succeeds at gateway, but webhook fails to reach app. User is charged but balance not updated. Or webhook arrives twice, user credited twice.

**Business Impact**:
- User charged but no credit (angry users, chargebacks)
- User credited twice (company loses money)
- Payment processor penalties for disputes
- Support overhead handling disputes
- Regulatory scrutiny

**Technical Causes**:
- Webhook delivery not guaranteed
- No retry mechanism for failed webhooks
- Duplicate webhook not detected (no idempotency)
- Network timeout during webhook processing
- No manual reconciliation process


**Mitigation Plan**:
1. **Idempotency**: Use unique transaction IDs, ignore duplicate webhooks
2. **Retry Logic**: Webhook processor retries failed updates
3. **Polling**: Background job polls payment gateway for status updates
4. **Reconciliation**: Daily reconciliation (gateway records vs. app records)
5. **Manual Process**: Finance team can manually credit user with proof of payment
6. **Monitoring**: Alerts for webhook failures
7. **Testing**: Test webhook retry, duplicate, and failure scenarios


#### **R-004: Race Conditions in Trading**
**Impact**: 5 | **Probability**: 3 | **Score**: 15 | **Level**: 🟠 HIGH

**Description**:  
User places two orders simultaneously, only one executes but both are charged. Stock price changes between order placement and execution, user gets wrong price. Concurrent trades cause balance to go negative.

**Business Impact**:
- Financial disputes with users
- Incorrect balances
- Regulatory issues (unfair pricing)
- Loss of user trust

**Technical Causes**:
- No database-level locking on balance updates
- No optimistic/pessimistic locking on orders
- Insufficient funds check not atomic
- Race condition between price fetch and order execution


**Mitigation Plan**:
1. **Database Locking**: Use row-level locking for balance updates
2. **Optimistic Locking**: Use version numbers to detect concurrent updates
3. **Atomic Operations**: Insufficient funds check and debit in single transaction
4. **Idempotency**: Order submission uses unique client-generated ID
5. **Load Testing**: Test with 100+ concurrent users placing orders
6. **Testing**: Test rapid-fire order placement scenarios

---

## Risk Mitigation Summary

### By Priority

**🔴 CRITICAL (Release Blockers)**:
- R-001: Financial loss bugs → Must test exhaustively before release

**🟠 HIGH (Urgent Mitigation Required)**:
- R-002: Security breach → Penetration testing required
- R-003: Payment gateway failures → Reconciliation process required
- R-004: Race conditions → Load testing required
- R-006: Catastrophic crashes → Device testing required
- R-007: Performance degradation → Performance testing required
- R-008: Data sync issues → Multi-device testing required
- R-009: Third-party API failures → Graceful degradation required
- R-010: No rollback/hotfix plan → Plan and test required
- R-011: Device compatibility → Real device testing required
- R-012: Network failures → Offline mode testing required

**🟡 MEDIUM (Monitor & Mitigate)**:
- R-005, R-013, R-014, R-015, R-016, R-017, R-018


## GO/NO-GO Criteria

**🔴 NO-GO if**:
- Any CRITICAL risk unmitigated
- More than 3 HIGH risks unmitigated
- Security audit not completed
- No rollback/hotfix plan

**🟡 GO WITH RISK if**:
- All CRITICAL risks mitigated
- Up to 3 HIGH risks accepted with documented mitigation plan
- Phased rollout strategy in place

**✅ GO if**:
- All CRITICAL risks mitigated
- All HIGH risks mitigated to MEDIUM or below
- Monitoring and alerting in place
- Support team ready


**Document Owner**: QA Lead + Engineering Lead  
**Review Frequency**: Weekly until release, then monthly  
**Last Updated**: September 6, 2026
