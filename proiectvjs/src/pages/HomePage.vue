<template>
  <div class="posts-scroll-container">
    <div class="posts-list">
      <PostCard v-for="(post, idx) in posts" :key="idx" :post="post" />
    </div>
  </div>
</template>

<script>
import PostCard from '../components/PostCard.vue'

export default {
  components: { PostCard },
  data() {
    return {
      posts: []
    }
  },
  mounted() {
    this.fetchPosts();
  },
  methods: {
    async fetchPosts() {
      try {
        const [tests, lessons] = await Promise.all([
          fetch(`${import.meta.env.VITE_API_URL}/Tests`),
          fetch(`${import.meta.env.VITE_API_URL}/Lessons`)
        ]);

        const testsData = await tests.json();
        const lessonsData = await lessons.json();

        const formattedTests = testsData.map(t => ({
          title: t.title,
          description: t.description || "Test publicat.",
          author: null,
          type: "Test",
          attachment: t.attachment || null
        }));

        const formattedLessons = lessonsData.map(l => ({
          title: l.title,
          description: l.description || "Material disponibil.",
          author: "Prof. " + (l.author || "Necunoscut"),
          type: l.type || "Curs",
          attachment: l.attachment || null
        }));

        this.posts = [...formattedTests, ...formattedLessons];
      } catch (err) {
        console.error("Eroare la încărcarea postărilor:", err);
      }
    }
  }
}
</script>

<style src="../assets/homepage.css"></style>
