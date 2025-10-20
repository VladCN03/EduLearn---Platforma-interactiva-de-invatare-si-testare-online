<template>
  <div class="statistics-main">
    <div class="profile-section">
      <h2>Profil utilizator</h2>
      <div class="profile-card">
        <img v-if="user.avatar" :src="user.avatar" alt="Avatar" class="avatar" />
        <div>
          <div><b>Nume:</b> {{ user.name }}</div>
          <div><b>Email:</b> {{ user.email }}</div>
          <div><b>Rol:</b> {{ user.role }}</div>
        </div>
      </div>
    </div>

    <div class="statistics-sections-row">
      <div class="stat-section">
        <h3>Teste finalizate</h3>
        <ul>
          <li v-for="t in completedTests" :key="t.title">
            <b>{{ t.title }}</b> (Scor: {{ t.score }}/{{ t.maxGrade }})
          </li>
          <li v-if="!completedTests.length" class="no-data">Niciun test finalizat.</li>
        </ul>
      </div>

      <div class="stat-section">
        <h3>Cursuri parcurse</h3>
        <ul>
          <li v-for="c in completedCourses" :key="c.title">
            <b>{{ c.title }}</b> - {{ c.author }}
          </li>
          <li v-if="!completedCourses.length" class="no-data">Niciun curs parcurs.</li>
        </ul>
      </div>

<!--      <div class="stat-section">-->
<!--        <h3>Postări create</h3>-->
<!--        <ul>-->
<!--          <li v-for="p in posts" :key="p.id">-->
<!--            <b>{{ p.title }}</b> ({{ formatDate(p.date) }})-->
<!--          </li>-->
<!--          <li v-if="!posts.length" class="no-data">Nicio postare creată.</li>-->
<!--        </ul>-->
<!--      </div>-->
    </div>

    <div class="statistics-sections-row">
      <div class="stat-section">
        <h3>Comentarii plasate</h3>
        <ul>
          <li v-for="com in comments" :key="com.id">
            <b>{{ com.section }}</b>: "{{ com.text }}" <span class="com-date">({{ formatDate(com.date) }})</span>
          </li>
          <li v-if="!comments.length" class="no-data">Niciun comentariu plasat.</li>
        </ul>
      </div>
      <div class="stat-section">
        <h3>Scoruri test (grafic)</h3>
        <!-- Poți pune un grafic real cu chart.js sau recharts in React. Aici doar text -->
        <div v-if="completedTests.length">
          <div v-for="t in completedTests" :key="t.title" class="score-bar-row">
            <span class="score-label">{{ t.title }}</span>
            <div class="score-bar">
              <div class="score-fill" :style="{width: ((t.score / t.maxGrade) * 100) + '%' }"></div>
            </div>
            <span class="score-value">{{ t.score }}/{{ t.maxGrade }}</span>
          </div>
        </div>
        <div v-else class="no-data">Nicio notă disponibilă.</div>
      </div>
      <!-- Poți adăuga și alte secțiuni (medalii, progres lunar, etc) -->
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: "Statistics",
  props: {
    user: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      completedTests: [],
      completedCourses: [],
      comments: []
    };
  },
  async mounted() {
    const userId = this.user.userID;

    try {
      const [testResultsRes, lessonsRes, testCommentsRes, testsRes] = await Promise.all([
        axios.get(`/api/TestResults/byUser/${userId}`),
        axios.get(`/api/CompletedLessons/byUser/${userId}`),
        axios.get(`/api/Tests/comments/byUser/${userId}`),
        axios.get(`/api/Tests`)
      ]);

      const testsMap = {};
      testsRes.data.forEach(t => {
        testsMap[t.testID] = {
          title: t.title,
          questions: JSON.parse(t.questions || "[]")
        };
      });

      this.completedTests = testResultsRes.data.map(result => {
        const test = testsMap[result.testID];
        return {
          title: test?.title || "Test necunoscut",
          score: result.score,
          maxGrade: test?.questions?.length || result.maxGrade || 0,
          date: result.completedAt
        };
      });

      this.completedCourses = lessonsRes.data
          .filter(l => l && l.title)
          .map(l => ({
            title: l.title,
            author: l.author,
            date: l.completedAt
          }));

      this.comments = (testCommentsRes.data || []).flatMap(c => {
        try {
          const parsed = JSON.parse(c.comment);
          return parsed
              .filter(p => p.text && p.text.trim() !== "")
              .map(pc => ({
                id: c.testTitle,
                section: `Test: ${c.testTitle}`,
                text: pc.text,
                date: c.submittedAt
              }));
        } catch (e) {
          console.error("Eroare la parsarea comentariilor:", e);
          return [];
        }
      });

    } catch (err) {
      console.error("Eroare la încărcarea datelor de statistică:", err);
    }
  },
  methods: {
    formatDate(dateStr) {
      const d = new Date(dateStr);
      return d.toLocaleDateString('ro-RO', { day: '2-digit', month: 'short', year: 'numeric' });
    }
  }
}
</script>

<style scoped>
.statistics-main {
  overflow: hidden;
  display: flex;
  flex-direction: column;
  max-width: 1150px;
  margin: 0 auto;
  padding: 32px 0 0 0;
  font-family: 'Inter', sans-serif;
}
.profile-section h2 {
  color: #1976d2;
  font-size: 1.25rem;
  margin-bottom: 12px;
}
.profile-card {
  display: flex;
  align-items: center;
  gap: 24px;
  background: #e9f7ff;
  border-radius: 14px;
  box-shadow: 0 4px 18px #6bc1ff13;
  padding: 18px 28px;
  margin-bottom: 28px;
}
.avatar {
  width: 70px;
  height: 70px;
  border-radius: 50%;
  object-fit: cover;
  background: #f5f8fa;
  border: 2px solid #b8e3ff;
}
.statistics-sections-row {
  display: flex;
  flex-direction: row;
  gap: 26px;
  margin-bottom: 28px;
}
.stat-section {
  flex: 1;
  width: 100%;
  background: #fff;
  border-radius: 14px;
  box-shadow: 0 4px 18px #6bc1ff11;
  border: 1px solid #e0e7ef;
  padding: 16px 22px;
  min-height: 170px;
  display: flex;
  flex-direction: column;
}
.stat-section h3 {
  margin-bottom: 11px;
  color: #267be8;
  font-size: 1.08rem;
}
.no-data {
  color: #a0a4aa;
  font-style: italic;
}
.score-bar-row {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 8px;
}
.score-label {
  flex: 1;
  font-size: 0.98rem;
}
.score-bar {
  flex: 2;
  background: #f0f6fd;
  border-radius: 8px;
  height: 18px;
  overflow: hidden;
  margin-right: 8px;
  min-width: 90px;
}
.score-fill {
  height: 100%;
  background: #6bc1ff;
  border-radius: 8px;
  transition: width 0.5s;
}
.score-value {
  font-weight: 600;
  color: #1976d2;
}
.com-date {
  color: #8aa0b5;
  font-size: 0.95em;
  margin-left: 7px;
}
</style>
