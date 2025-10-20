  <template>
    <div class="tests-main">
      <!-- LISTĂ TESTE -->
      <template v-if="activeTestIndex === null">


        <!-- Buton adăugare test pentru profesori -->
        <div v-if="user && user.role === 'Profesor'">
          <button class="create-btn" @click="showAddTestModal = true">📃 Adaugă test</button>
        </div>

          <h2 class="">Teste</h2>
          <div class="">
            <div v-for="(test, idx) in filteredTests" :key="idx" class="test-item">
            <div class="test-summary" @click="toggleTest(idx)">
              <span class="test-title">{{ test.title }}</span>
              <span class="test-rating-preview">★ {{ test.rating }}/5</span>
              <span
                  class="test-status status-transition"
                  :class="test.status === 'activ' ? 'active' : 'inactive'"
                  @click.stop="user && user.role === 'Profesor' && toggleStatus(idx)"
                  :title="user && user.role === 'Profesor' ? 'Click pentru a schimba statusul' : ''"
              >
              {{ test.status === 'activ' ? 'Activ' : 'Inactiv' }}
            </span>

            </div>
            <div v-if="expandedIndex === idx" class="test-details">
              <p class="test-desc">{{ test.description }}</p>
              <div class="test-info-grid">
                <div><b>Durată:</b> {{ test.duration }} min</div>
                <div><b>Nr. întrebări:</b> {{ test.questions.length }}</div>
                <div><b>Notă maximă:</b> {{ test.maxGrade }}</div>
                <div>
                  <b>Rating:</b>
                  <span class="rating-stars">
                    <span
                      v-for="star in 5"
                      :key="star"
                      class="star"
                      :class="{ filled: star <= test.rating }"
                      @click.stop="rateTest(idx, star)"
                      style="cursor: pointer;"
                      >★</span>
                    <span class="rating-value" v-if="test">({{ test.rating }}/5)</span>
                  </span>
                </div>
              </div>
              <div v-if="test.attachment" class="test-attachment">
                <span>📎 <a :href="`/fisiere/${test.attachment}`" target="_blank">{{ test.attachment }}</a></span>
              </div>
              <div class="test-comments">
                <div class="comments-title"><b>Comentarii utilizatori:</b></div>
                <ul>
                  <li v-for="(c, cidx) in test.comments" :key="cidx">
                    <span class="comment-author">{{ c.author }}</span>: {{ c.text }}
                  </li>
                  <li v-if="!test.comments.length" class="no-comments">Niciun comentariu încă.</li>
                </ul>
              </div>
              <div class="test-actions">
                <button
                    v-if="test.status === 'activ'"
                    @click.stop="startTest(idx)"
                >Începe testul</button>
                <button v-if="test.status === 'inactiv'" disabled>Test indisponibil</button>
                <button
                    @click="downloadAttachment(test)"
                    :disabled="!test.attachment || test.status !== 'activ'"
                >
                  Descarcă atașament
                </button>
                <div v-if="user && user.role === 'Profesor'" class="edit-delete-buttons">
                  <button @click.stop="editTest(idx)">✏️ Editează</button>

                  <button @click.stop="deleteTest(idx)">🗑️ Șterge</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </template>

      <!-- PAGINA TEST ACTIV -->
      <template v-else>
        <div class="testpage" v-if="currentTest">
          <button class="back-btn" @click="activeTestIndex = null">← Înapoi la teste</button>
          <h2>{{ currentTest.title }}</h2>

          <div class="test-rating">
            <b>Rating:</b>
            <span class="rating-stars">
            <span v-for="star in 5" :key="star" class="star"
              :class="{ filled: star <= currentTest.rating }"
              @click="rateTest(activeTestIndex,star)"
            >★</span>
            <span class="rating-value">({{ currentTest.rating }}/5)</span>
            </span>
          </div>
          <div class="questions-section">
            <div
                v-for="(q, qIdx) in currentTest.questions"
                :key="qIdx"
                class="question-item"
            >
              <div class="question-text">{{ qIdx + 1 }}. {{ q.text }}</div>
              <div v-if="q.options" class="question-options">
                <label v-for="(option, oIdx) in q.options" :key="oIdx">
                  <input
                      type="radio"
                      :name="'q_' + qIdx"
                      :value="option"
                      v-model="answers[qIdx]"
                  >
                  {{ option }}
                </label>
              </div>
              <div v-else-if="q.type === 'text'">
                <input type="text" v-model="answers[qIdx]" placeholder="Răspunsul tău" />
              </div>
            </div>
          </div>
          <button class="submit-btn" @click="submitTest">Trimite testul</button>
          <!-- SECȚIUNE COMENTARII -->
          <div class="test-comments" style="margin-top:32px;">
            <div class="comments-title"><b>Comentarii utilizatori:</b></div>
            <ul>
              <li v-for="(c, cidx) in currentTest.comments" :key="cidx">
                <span class="comment-author">{{ c.author }}</span>: {{ c.text }}
              </li>
              <li v-if="!currentTest.comments.length" class="no-comments">Niciun comentariu încă.</li>
            </ul>
            <div class="add-comment">
              <input v-model="newComment" placeholder="Adaugă un comentariu..." @keyup.enter="addComment" />
              <button @click="addComment">Adaugă</button>
            </div>
          </div>
        </div>
      </template>
      <!-- MODAL ADAUGARE TEST -->
      <div v-if="showAddTestModal" class="modal-overlay">
        <div class="modal-container">
          <h3>Adaugă test nou</h3>
          <div class="test-form-row">
            <input class="test-input" v-model="newTest.title" placeholder="Titlu test" />
          </div>

          <div class="test-form-row">
            <select class="test-input" v-model="newTest.status">
              <option value="activ">Activ</option>
              <option value="inactiv">Inactiv</option>
            </select>
          </div>

          <div class="test-form-row">
            <input
                class="test-input"
                type="file"
                @change="handleTestFile"
            />
          </div>

          <div class="test-form-row">
            <input
                class="test-input"
                type="number"
                v-model="newTest.duration"
                placeholder="Durata (minute)"
                min="1"
            />
          </div>

          <div class="test-form-row">
            <label for="rating"> Rating </label>
            <input
                class="test-input"
                type="number"
                v-model="newTest.rating"
                placeholder="Rating (1-5)"
                min="1"
                max="5"
            />
          </div>

          <div class="test-form-row">
            <label for="max-grade"> Notă maximă </label>
            <input
                class="test-input"
                type="number"
                v-model="newTest.maxGrade"
                placeholder="Notă maximă"
                min="1"
            />
          </div>

          <h4 class="question-title">Întrebări</h4>
          <div v-for="(q, idx) in newTest.questions" :key="idx" class="question-block">
            <div class="question-header">
              <label class="question-label">Întrebarea {{ idx + 1 }}</label>
              <button class="delete-question-btn" @click="removeQuestion(idx)">✖</button>
            </div>

            <input class="question-input" v-model="q.text" placeholder="Text întrebare" />

            <div v-for="(opt, oIdx) in q.options" :key="oIdx">
              <input
                  class="question-input"
                  v-model="q.options[oIdx]"
                  :placeholder="`Opțiunea ${String.fromCharCode(65 + oIdx)}`"
              />
            </div>

            <input
                class="question-input"
                v-model="q.answer"
                placeholder="Răspuns corect (ex: exact ca opțiunea)"
            />
          </div>

          <button class="add-question-btn" @click="addNewQuestion">➕ Adaugă întrebare</button>

          <div class="modal-buttons">
            <button class="save" @click="createTest">Salvează testul</button>
            <button class="cancel" @click="cancelForm">Anulează</button>
          </div>
        </div>
      </div>
    </div>
  </template>

  <script>
  export default {
    data() {
      return {
        user: (() => {
          try {
            const saved = localStorage.getItem('user');
            return saved ? JSON.parse(saved) : null;
          } catch (e) {
            return null;
          }
        })(),
        showAddTestModal: false,
        newTest: {
          title: '',
          status: 'activ',
          attachment: null,
          duration: null,
          rating: 0,
          maxGrade: 10,  // ← valoare implicită
          questions: []
        },
        editingTestId: null,
        expandedIndex: null,
        activeTestIndex: null,
        searchTerm: "",
        filterStatus: "",
        filterAttachment: false,
        filterRated5: false,
        answers: [],
        newComment: "",
        tests: []
      };
    },
    mounted() {
      this.fetchTests();
    },
    computed: {
      filteredTests() {
        return this.tests.filter(t =>
            (!this.searchTerm || t.title.toLowerCase().includes(this.searchTerm.toLowerCase()) || t.description.toLowerCase().includes(this.searchTerm.toLowerCase()))
            && (!this.filterStatus || t.status === this.filterStatus)
            && (!this.filterAttachment || !!t.attachment)
            && (!this.filterRated5 || t.rating === 5)
        );
      },
      currentTest() {
        if (
            this.activeTestIndex !== null &&
            Array.isArray(this.tests) &&
            this.tests[this.activeTestIndex]
        ) {
          return this.tests[this.activeTestIndex];
        }
        return null;
      }
    },
    methods: {
      cancelForm() {
        this.showAddTestModal = false;
        this.editingTestId = null;
        this.newTest = {
          title: '',
          status: 'activ',
          attachment: null,
          duration: null,
          rating: 0,
          maxGrade: 10,
          questions: [],
          comments: []
        };
      },
      addNewQuestion() {
        if (!this.newTest.questions) {
          this.newTest.questions = [];
        }

        this.newTest.questions.push({
          text: '',
          options: ['', '', '', ''],
          answer: ''
        });
      },
      handleTestFile(e) {
        const file = e.target.files[0];
        if (file) {
          this.newTest.attachment = file.name;
        }
      },
      async fetchTests() {
        try {
          const res = await fetch(`${import.meta.env.VITE_API_URL}/Tests`);
          const data = await res.json();

          for (const test of this.tests) {
            const res = await fetch(`${import.meta.env.VITE_API_URL}/TestResults/maxGrade/${test.testID}`);
            const maxGrade = await res.json();
            test.maxGrade = maxGrade || 10; // fallback dacă nu există
          }

          this.tests = data.map(test => ({
            ...test,
            rating: test.rating || 0,
            comments: test.comments ? JSON.parse(test.comments) : [],
            questions: test.questions ? JSON.parse(test.questions) : [],
            maxGrade: test.maxGrade || 10
          }));
        } catch (err) {
          console.error("Eroare la încărcarea testelor:", err);
        }
      },
      toggleTest(idx) {
        this.expandedIndex = this.expandedIndex === idx ? null : idx;
      },
      startTest(idx) {
        this.activeTestIndex = idx;
        this.answers = [];
        this.newComment = "";
      },
      calculateNote(score, totalQuestions, maxGrade) {
        return Math.round((score / totalQuestions) * maxGrade);
      },
      async submitTest() {
        try {
          const user = JSON.parse(localStorage.getItem('user'));
          const test = this.currentTest;

          const rawScore = this.calculateScore(test);
          const totalQuestions = test.questions.length;
          const maxGrade = test.maxGrade || 10;

          const finalGrade = Math.round((rawScore / totalQuestions) * maxGrade);

          const rezultat = {
            userID: user.userID,
            testID: test.testID,
            score: rawScore,
            maxGrade: maxGrade,
            completedAt: new Date().toISOString()
          };

          await fetch(`${import.meta.env.VITE_API_URL}/TestResults`, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(rezultat)
          });

          alert(`Testul a fost trimis cu succes!\nAi obținut nota ${finalGrade}/${maxGrade} (${rawScore} răspunsuri corecte din ${totalQuestions})`);
          this.activeTestIndex = null;
        } catch (err) {
          console.error("Eroare la trimiterea testului:", err);
        }
      },
      calculateScore(test) {
        let puncte = 0;
        test.questions.forEach((q, i) => {
          if (q.answer && this.answers[i] === q.answer) {
            puncte++;
          }
        });
        return puncte;
      },
      addComment() {
        if (!this.newComment.trim()) return;

        const user = JSON.parse(localStorage.getItem('user'));
        const newComm = {
          author: user.name,
          text: this.newComment.trim()
        };

        this.currentTest.comments.push(newComm);
        this.newComment = "";

        // Salvăm comentariile înapoi în baza de date
        fetch(`${import.meta.env.VITE_API_URL}/Tests/${this.currentTest.testID}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            ...this.currentTest,
            comments: JSON.stringify(this.currentTest.comments),
            questions: JSON.stringify(this.currentTest.questions)  // păstrăm și întrebările existente
          })
        }).then(res => {
          if (!res.ok) throw new Error('Eroare la salvarea comentariului');
        }).catch(err => {
          console.error("Eroare la salvare comentariu:", err);
        });
      },
      async createTest() {
        const payload = {
          ...this.newTest,
          questions: JSON.stringify(this.newTest.questions),
          comments: JSON.stringify(this.newTest.comments),
          maxGrade: this.newTest.maxGrade
        };

        const method = this.editingTestId ? 'PUT' : 'POST';
        const url = this.editingTestId
            ? `${import.meta.env.VITE_API_URL}/Tests/${this.editingTestId}`
            : `${import.meta.env.VITE_API_URL}/Tests`;

        try {
          const res = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
          });

          if (res.ok) {
            alert(this.editingTestId ? "Testul a fost actualizat." : "Test adăugat!");
            this.showAddTestModal = false;
            this.editingTestId = null;
            this.fetchTests();
          }
        } catch (err) {
          console.error("Eroare la salvare test:", err);
        }
      },
      rateTest(index, star) {
        if (!this.tests[index]) return;

        // Actualizare locală
        this.tests[index].rating = star;

        // Salvează modificarea și în backend
        fetch(`${import.meta.env.VITE_API_URL}/Tests/${this.tests[index].testID}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            ...this.tests[index],
            comments: JSON.stringify(this.tests[index].comments),
            questions: JSON.stringify(this.tests[index].questions)
          })
        }).then(res => {
          if (!res.ok) throw new Error('Ratingul nu a putut fi salvat.');
        }).catch(err => {
          console.error('Eroare la rating:', err);
        });
      },
      async toggleStatus(index) {
        const test = this.tests[index];
        if (!test) return;

        // Inversăm statusul local
        test.status = test.status === 'activ' ? 'inactiv' : 'activ';

        try {
          await fetch(`${import.meta.env.VITE_API_URL}/Tests/${test.testID}`, {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({
              ...test,
              comments: JSON.stringify(test.comments),
              questions: JSON.stringify(test.questions)
            })
          });
        } catch (err) {
          console.error("Eroare la actualizarea statusului:", err);
          alert("Eroare la salvarea noului status.");
        }
      },
      downloadAttachment(test) {
        const link = document.createElement('a');
        link.href = `/fisiere/${test.attachment}`;
        link.download = test.attachment;
        link.target = "_blank";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      },
      removeQuestion(index) {
        if (confirm("Sigur vrei să ștergi această întrebare?")) {
          this.newTest.questions.splice(index, 1);
        }
      },
      async deleteTest(index) {
        const test = this.tests[index];
        if (confirm(`Sigur vrei să ștergi testul "${test.title}"?`)) {
          try {
            const res = await fetch(`${import.meta.env.VITE_API_URL}/Tests/${test.testID}`, {
              method: 'DELETE'
            });

            if (res.ok) {
              this.tests.splice(index, 1);
              alert("Testul a fost șters.");
            } else {
              alert("Eroare la ștergere test.");
            }
          } catch (err) {
            console.error("Eroare la ștergere test:", err);
          }
        }
      },
      editTest(index) {
        const test = this.tests[index];
        this.newTest = {
          ...test,
          questions: [...test.questions],
          comments: [...test.comments]
        };
        this.editingTestId = test.testID; // pentru a ști dacă salvezi modificări
        this.showAddTestModal = true;
      }
    }
  }
  </script>
  <style scoped>
  .star {
    font-size: 20px;
    color: #ccc;
    transition: color 0.3s;
  }

  .star.filled {
    color: gold;
  }
  .create-btn {
    background-color: #1976d2;
    color: white;
    padding: 8px 14px;
    border-radius: 8px;
    border: none;
    cursor: pointer;
  }

  .modal-overlay {
    position: fixed;
    top: 0; left: 0; right: 0; bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    z-index: 1000;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .modal-box {
    background: white;
    padding: 24px;
    width: 500px;
    max-width: 90%;
    border-radius: 12px;
    box-shadow: 0 0 20px rgba(0,0,0,0.2);
  }

  .modal-buttons {
    margin-top: 16px;
    display: flex;
    justify-content: flex-end;
    gap: 10px;
  }

  .modal-buttons button {
    padding: 8px 12px;
    border-radius: 6px;
    border: none;
    cursor: pointer;
  }

  .modal-buttons .save {
    background-color: #1976d2;
    color: white;
  }

  .modal-buttons .cancel {
    background-color: #ccc;
  }

  .test-status {
    padding: 4px 10px;
    border-radius: 12px;
    font-size: 13px;
    font-weight: bold;
    margin-left: 8px;
    display: inline-block;
  }

  .test-status.active {
    background-color: #d4edda;
    color: #2e7d32;
  }

  .test-status.inactive {
    background-color: #f8d7da;
    color: #b71c1c;
  }

  .test-form-row {
    margin-bottom: 12px;
  }

  .test-input {
    width: 100%;
    padding: 8px 12px;
    border: 1px solid #cfd8dc;
    border-radius: 6px;
    font-size: 1rem;
    font-family: 'Inter', sans-serif;
    box-sizing: border-box;
    background-color: #fff;
  }

  .question-title {
    margin-top: 20px;
    margin-bottom: 12px;
    font-size: 1.15rem;
    color: #1976d2;
  }

  .question-block {
    background: #f9fcff;
    padding: 18px 25px;
    border: 1px solid #cde0ef;
    border-radius: 10px;
    margin-bottom: 16px;
    box-shadow: 0 2px 8px #6bc1ff22;
  }

  .question-label {
    display: block;
    margin-bottom: 8px;
    font-weight: 600;
    color: #34434c;
  }

  .question-input {
    width: 100%;
    margin-bottom: 10px;
    padding: 8px 12px;
    border: 1px solid #ccddea;
    border-radius: 6px;
    font-size: 1rem;
    font-family: 'Inter', sans-serif;
    box-sizing: border-box;
  }

  .add-question-btn {
    background-color: #1976d2;
    color: white;
    padding: 8px 14px;
    border-radius: 8px;
    border: none;
    font-weight: 500;
    cursor: pointer;
    transition: background-color 0.2s;
  }

  .add-question-btn:hover {
    background-color: #125ea0;
  }

  .modal-container {
    background: white;
    padding: 60px;
    border-radius: 10px;
    max-height: 85vh;
    overflow-y: auto;
  }


  </style>

  <style scoped src="../assets/testspage.css"></style>
