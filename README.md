# My2D · 2D 게임 실습 모음

하나의 Unity 프로젝트 안에 2D 액션과 MyBird 실습을 함께 보관합니다.

## 확인할 기능

- 2D 액션: 이동·달리기·점프와 공격
- 2D 액션: 피해·감지 영역과 애니메이션 상태 연결
- MyBird: 플레이어와 장애물 생성 및 점수·UI 관리

## 주요 코드와 구조

| 경로 | 역할 |
| --- | --- |
| [Assets/My2DGame/Scripts](Assets/My2DGame/Scripts) | 2D 액션 코드 |
| [Assets/My2DGame/Scenes/PlayScene.unity](Assets/My2DGame/Scenes/PlayScene.unity) | 2D 액션 씬 |
| [Assets/MyBird/Scripts](Assets/MyBird/Scripts) | MyBird 코드 |
| [Assets/MyBird/Scenes/SampleScene.unity](Assets/MyBird/Scenes/SampleScene.unity) | MyBird 씬 |
| [ProjectSettings/EditorBuildSettings.asset](ProjectSettings/EditorBuildSettings.asset) | 현재 빌드 씬 설정 |

## 실행 방법

1. Unity Hub에서 저장소 폴더를 프로젝트로 추가합니다.
2. **Unity 6000.3.13f1**로 열고 패키지 복원과 임포트를 기다립니다.
3. [시작 씬](Assets/MyBird/Scenes/SampleScene.unity)을 엽니다.
4. Play Mode에서 동작을 확인합니다.

현재 빌드 설정에는 MyBird 씬만 등록되어 있습니다. 2D 액션은 해당 PlayScene을 직접 열어 확인하며 빌드에 포함하려면 빌드 씬 설정을 별도로 확인해야 합니다.

## 작업 범위와 검증

수업 기반 학습 프로젝트입니다. 위 목록은 저장소의 코드 구성에 근거한 기능 설명이며 수업 예제와 개인 확장 기능의 구분은 별도 작업 기록을 기준으로 합니다. 이번 문서 정리는 새로 수행한 실행·빌드 시험의 결과를 의미하지 않습니다.

외부 에셋과 샘플은 자체 제작 코드와 구분하며 각 리소스의 라이선스를 따릅니다.
