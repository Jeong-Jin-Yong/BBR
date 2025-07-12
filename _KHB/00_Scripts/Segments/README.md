# 플랫폼 모듈 시스템

이 시스템은 Unity에서 플랫폼 게임의 세그먼트를 오브젝트 풀링을 통해 효율적으로 생성하고 관리하는 시스템입니다.

## 주요 기능

### 1. 시작 플랫폼 자동 생성
- 게임 시작 시 무조건 StartPlatform 프리팹이 첫 번째로 등장
- 이후 고정 패턴으로 플랫폼이 배치됨

### 2. 고정 패턴 시스템
- 점프 → 슬라이딩 → 더블점프 → 점프 순서로 반복
- 패턴: `{ 1, 2, 3, 1 }` (점프, 슬라이딩, 더블점프, 점프)

### 3. 오브젝트 풀링
- 메모리 효율적인 세그먼트 관리
- 타입별 풀링으로 빠른 세그먼트 재사용

## 설정 방법

### 1. SegmentPool 설정
```
Segment Prefabs 배열에 다음 순서로 프리팹을 추가:
- Index 0: StartPlatform (시작 플랫폼)
- Index 1: JumpPlatform (점프 플랫폼)
- Index 2: SlidingPlatform (슬라이딩 플랫폼)
- Index 3: DoubleJumpPlatform (더블점프 플랫폼)
```

### 2. SegmentGenerator 설정
```
Platform Pattern 섹션에서 인덱스 설정:
- Start Platform Index: 0
- Jump Platform Index: 1
- Sliding Platform Index: 2
- Double Jump Platform Index: 3
```

## 사용법

### 1. 기본 사용
```csharp
// 게임 시작 시 자동으로 시작 플랫폼과 패턴이 생성됩니다
// 별도의 설정 없이도 작동합니다
```

### 2. 패턴 변경
```csharp
// 런타임에 패턴 변경 가능
int[] newPattern = { 1, 1, 2, 3 }; // 점프-점프-슬라이딩-더블점프
SegmentGenerator.Instance.SetPlatformPattern(newPattern);
```

### 3. 난이도 조정
```csharp
// 난이도 설정 (1-5)
SegmentGenerator.Instance.SetDifficulty(3);

// 이동 속도 설정
SegmentGenerator.Instance.SetMoveSpeed(8f);
```

### 4. 게임 리셋
```csharp
// 게임 재시작 시 모든 세그먼트 초기화
SegmentGenerator.Instance.ResetGenerator();
```

## 파일 구조

- `SegmentGenerator.cs`: 세그먼트 생성 및 패턴 관리
- `SegmentPool.cs`: 오브젝트 풀링 시스템
- `Segment.cs`: 개별 세그먼트 컴포넌트
- `SegmentType.cs`: 세그먼트 타입 열거형

## 성능 최적화

1. **타입별 풀링**: 각 세그먼트 타입별로 별도 풀 관리
2. **메모리 효율성**: 세그먼트 재사용으로 GC 부하 최소화
3. **자동 정리**: 화면 밖으로 나간 세그먼트 자동 반환

## 주의사항

1. 프리팹 배열의 인덱스 순서가 중요합니다
2. 세그먼트 프리팹에 Segment 컴포넌트가 있어야 합니다
3. 세그먼트의 startPoint와 endPoint가 올바르게 설정되어야 합니다 