<template>
  <div class="lessons-page">
    <div class="header">
      <h2>Cursuri</h2>
      <button v-if="props.user.role === 'Profesor'" @click="toggleCourseForm" class="btn-primary">Add course</button>
    </div>

    <form v-if="showAddCourseForm && props.user.role === 'Profesor'" @submit.prevent="addCourse" class="add-form">
      <input v-model="courseTitle" placeholder="Titlul cursului" required />
      <textarea v-model="courseDescription" placeholder="Descriere..." required />
      <input type="file" @change="handleCourseFile" />
      <button type="submit" class="btn-submit">Salvează</button>
    </form>

    <div v-for="(course, index) in courses" :key="index" class="lesson-item">
      <div class="lesson-summary" @click.stop.prevent="toggleDetails(index, 'course')">
      <span class="lesson-title">
      {{ course.title }}
      <span class="check-icon">✔</span>
      </span>
        <span class="lesson-author">({{ course.author }})</span>
        <span class="lesson-date">{{ formatDate(course.date) }}</span>
      </div>
      <div class="lesson-interactions" v-if="props.user.role === 'Profesor'">
        <button @click.stop="editCourse(course)">Editează</button>
        <button @click.stop="deleteLesson(course.lessonID)">Șterge</button>
      </div>
      <div v-if="expanded.course === index" class="lesson-details">
        <p>{{ course.description }}</p>
        <div v-if="course.attachment" class="lesson-attachment">
          📄 <a :href="`/fisiere/${course.attachment}`" target="_blank">{{ course.attachment }}</a>
          <div class="lesson-interactions">
            <button @click.stop="downloadAttachment(course.attachment)">Descarcă atașament</button>
          </div>
        </div>
      </div>
      <div class="lesson-completion">
        <button
            v-if="props.user.role === 'Student'"
            @click.stop="toggleCompleted(course)"
            :disabled="completedLessonIds.includes(Number(course.lessonID))"
            :class="completedLessonIds.includes(Number(course.lessonID)) ? 'completed' : 'not-completed'"
        >
          {{ completedLessonIds.includes(Number(course.lessonID)) ? '✔ Marcat' : '✓ Marchează ca parcurs' }}
        </button>
      </div>
    </div>


    <div class="header">
      <h2>Laboratoare</h2>
      <button v-if="props.user.role === 'Profesor'" @click="toggleCourseForm" class="btn-primary">Add laboratory</button>
    </div>

    <form v-if="showAddLabForm && props.user.role === 'Profesor'" @submit.prevent="addLaboratory" class="add-form">
      <input v-model="labTitle" placeholder="Titlul laboratorului" required />
      <textarea v-model="labDescription" placeholder="Descriere..." required />
      <input type="file" @change="handleLabFile" />
      <button type="submit" class="btn-submit">Salvează</button>
    </form>

    <div v-for="(lab, index) in labs" :key="index" class="lesson-item">
      <div class="lesson-summary" @click="toggleDetails(index, 'lab')">
        <div class="lesson-title">
          {{ lab.title }}
          <span class="check-icon">✔</span>
        </div>
        <span class="lesson-author">({{ lab.author }})</span>
        <span class="lesson-date">{{ formatDate(lab.date) }}</span>
      </div>
      <div class="lesson-interactions" v-if="props.user.role === 'Profesor'">
        <button @click.stop="editLab(lab)">Editează</button>
        <button @click.stop="deleteLesson(lab.lessonID)">Șterge</button>
      </div>
      <div v-if="expanded.lab === index" class="lesson-details">
        <p>{{ lab.description }}</p>
        <div v-if="lab.attachment" class="lesson-attachment">
          📄 <a :href="`/fisiere/${lab.attachment}`" target="_blank">{{ lab.attachment }}</a>
          <div class="lesson-interactions">
            <button @click.stop="downloadAttachment(lab.attachment)">Descarcă atașament</button>
          </div>
        </div>
      </div>
      <div class="lesson-completion">
        <button
            v-if="props.user.role === 'Student'"
            @click.stop="toggleCompleted(lab)"
            :disabled="completedLessonIds.includes(Number(lab.lessonID))"
            :class="completedLessonIds.includes(Number(lab.lessonID)) ? 'completed' : 'not-completed'"
        >
          {{ completedLessonIds.includes(Number(lab.lessonID)) ? '✔ Marcat' : '✓ Marchează ca parcurs' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const props = defineProps({
  user: {
    type: Object,
    required: true
  }
})

console.log("Props user:", props.user);

const courses = ref([]);
const labs = ref([]);

const showAddCourseForm = ref(false);
const showAddLabForm = ref(false);
const courseTitle = ref('');
const courseDescription = ref('');
const courseFile = ref(null);

const labTitle = ref('');
const labDescription = ref('');
const labFile = ref(null);


const expanded = ref({ course: null, lab: null });

const editingLessonId = ref(null);
const isEditingCourse = ref(false);
const isEditingLab = ref(false);

const fetchLessons = async () => {
  const res = await axios.get(`${import.meta.env.VITE_API_URL}/Lessons`);
  const all = res.data;
  courses.value = all.filter(l => l.type === 'curs');
  labs.value = all.filter(l => l.type === 'lab');
};

const handleCourseFile = e => {
  courseFile.value = e.target.files[0];
  console.log('Selected file:', courseFile.value);
};
const handleLabFile = e => labFile.value = e.target.files[0];

const toggleCourseForm = () => showAddCourseForm.value = !showAddCourseForm.value;
const toggleLabForm = () => showAddLabForm.value = !showAddLabForm.value;

const addCourse = async () => {
  const form = new FormData();
  form.append('Title', courseTitle.value);
  form.append('Description', courseDescription.value);
  form.append('Author', props.user.name);
  form.append('Date', new Date().toISOString());
  form.append('Type', 'curs');
  if (courseFile.value) form.append('File', courseFile.value);

  try {
    if (isEditingCourse.value && editingLessonId.value) {
      if (courseFile.value) {
        // cu fișier nou – POST ca FormData (modifici backend să accepte PUT cu form)
        await axios.put(`${import.meta.env.VITE_API_URL}/Lessons/${editingLessonId.value}`, form, {
          headers: { 'Content-Type': 'multipart/form-data' }
        });
      } else {
        // fără fișier – trimiți doar JSON
        await axios.put(`${import.meta.env.VITE_API_URL}/Lessons/${editingLessonId.value}`, {
          lessonID: editingLessonId.value,
          title: courseTitle.value,
          description: courseDescription.value,
          author: props.user.name,
          date: new Date().toISOString(),
          type: 'curs',
          attachment: null
        });
      }
    } else {
      await axios.post('/api/Lessons', form);
    }

    await fetchLessons();
    courseTitle.value = '';
    courseDescription.value = '';
    courseFile.value = null;
    showAddCourseForm.value = false;
    isEditingCourse.value = false;
    editingLessonId.value = null;
  } catch (err) {
    console.error('Eroare la salvare curs:', err);
  }
};

const addLaboratory = async () => {
  const form = new FormData();
  form.append('Title', labTitle.value);
  form.append('Description', labDescription.value);
  form.append('Author', props.user.name);
  form.append('Date', new Date().toISOString());
  form.append('Type', 'lab');
  if (labFile.value) form.append('File', labFile.value);

  try {
    if (isEditingLab.value && editingLessonId.value) {
      if (labFile.value) {
        // cu fișier nou – trimite FormData
        await axios.put(`${import.meta.env.VITE_API_URL}/Lessons/${editingLessonId.value}`, form, {
          headers: { 'Content-Type': 'multipart/form-data' }
        });
      } else {
        // fără fișier – trimite JSON simplu
        await axios.put(`${import.meta.env.VITE_API_URL}/Lessons/${editingLessonId.value}`, {
          lessonID: editingLessonId.value,
          title: labTitle.value,
          description: labDescription.value,
          author: props.user.name,
          date: new Date().toISOString(),
          type: 'lab',
          attachment: null
        });
      }
    } else {
      await axios.post('/api/Lessons', form);
    }

    await fetchLessons();
    labTitle.value = '';
    labDescription.value = '';
    labFile.value = null;
    showAddLabForm.value = false;
    isEditingLab.value = false;
    editingLessonId.value = null;
  } catch (err) {
    console.error('Eroare la salvare laborator:', err);
  }
};

const toggleDetails = (index, type) => {
  const alreadyExpanded = expanded.value[type] === index;
  expanded.value[type] = alreadyExpanded ? null : index;

  // când se deschide/închide, setează overflow pe body
  if (!alreadyExpanded) {
    document.body.classList.add('modal-open');
  } else {
    document.body.classList.remove('modal-open');
  }
};

const formatDate = dateStr => new Date(dateStr).toLocaleDateString('ro-RO');

const downloadAttachment = async (filename) => {
  try {
    const response = await axios.get(`/fisiere/${filename}`, {
      responseType: 'blob'
    });

    const blob = new Blob([response.data]);
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = filename; // păstrează extensia
    link.click();

    window.URL.revokeObjectURL(url);
  } catch (error) {
    console.error("Eroare la descărcare:", error);
  }
};

const editCourse = (course) => {
  courseTitle.value = course.title;
  courseDescription.value = course.description;
  editingLessonId.value = course.lessonID;
  showAddCourseForm.value = true;
  isEditingCourse.value = true;
};

const editLab = (lab) => {
  labTitle.value = lab.title;
  labDescription.value = lab.description;
  editingLessonId.value = lab.lessonID;
  showAddLabForm.value = true;
  isEditingLab.value = true;
};

const deleteLesson = async (id) => {
  if (!confirm("Ești sigur că vrei să ștergi această lecție?")) return;
  try {
    await axios.delete(`${import.meta.env.VITE_API_URL}/Lessons/${id}`);
    await fetchLessons();
  } catch (err) {
    console.error("Eroare la ștergere:", err);
  }
};

const completedLessonIds = ref([]);

const fetchCompletedLessons = async () => {
  try {
    const res = await axios.get(`/api/CompletedLessons`);
    console.log("Date completate primite:", res.data); // ADĂUGĂ AICI

    completedLessonIds.value = res.data
        .filter(cl => cl.user.userID === props.user.userID)
        .map(cl => Number(cl.lesson.lessonID));
  } catch (err) {
    console.error("Eroare la încărcarea CompletedLessons:", err);
  }
};

console.log("props.user.userID =", props.user.userID)

const toggleCompleted = async (lesson) => {
  const id = Number(lesson.lessonID);
  if (completedLessonIds.value.includes(id)) {
    console.log("Lecția e deja marcată, nu trimitem POST.");
    return;
  }

  try {
    const payload = {
      userID: props.user.userID,
      lessonID: id,
      completedAt: new Date().toISOString()
    };

    console.log("Trimitem payload:", payload);

    await axios.post(`${import.meta.env.VITE_API_URL}/CompletedLessons`, payload);
    await fetchCompletedLessons();
  } catch (err) {
    console.error("Eroare la schimbarea statusului lecției:", err);
  }
};

onMounted(async () => {
  await fetchLessons();
  await fetchCompletedLessons();
});
</script>

<style scoped>
.lessons-page {
  padding: 2rem;
}
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.btn-primary {
  background-color: #4fc3f7;
  border: none;
  padding: 0.5rem 1rem;
  color: white;
  border-radius: 6px;
  cursor: pointer;
  transition: 0.3s;
}
.btn-primary:hover {
  background-color: #039be5;
}
.add-form {
  margin: 1rem 0;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.add-form input, .add-form textarea {
  padding: 0.5rem;
  border-radius: 5px;
  border: 1px solid #ccc;
}
.btn-submit {
  align-self: flex-start;
  background-color: #00bcd4;
  border: none;
  color: white;
  padding: 0.4rem 1.2rem;
  border-radius: 4px;
  cursor: pointer;
}
.btn-submit:hover {
  background-color: #008c9e;
}
.lesson-item {
  background: #e3f2fd;
  padding: 1rem;
  margin-bottom: 1rem;
  border-radius: 8px;
  cursor: pointer;
}
.lesson-summary {
  display: flex;
  justify-content: space-between;
  font-size: 1.1rem;
}
.meta {
  font-style: italic;
  color: #555;
}
.lesson-details {
  margin-top: 0.8rem;
  max-height: 200px; /* sau cât vrei */
  overflow-y: auto;
  background: #ffffff;
  padding: 0.8rem;
  border-radius: 8px;
  box-shadow: 0 4px 14px rgba(0, 0, 0, 0.05);
}

.lesson-completion button {
  margin-top: 12px;
  padding: 6px 12px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
}

.lesson-completion .completed {
  background-color: #c8e6c9;
  color: #2e7d32;
}

.lesson-completion .not-completed {
  background-color: #e3f2fd;
  color: #0277bd;
}
.lesson-completion button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
<style src="../assets/lessonspage.css"></style>
