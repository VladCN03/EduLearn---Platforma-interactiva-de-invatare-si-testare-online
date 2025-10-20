<template>
  <div class="post-card">
    <div class="post-card-header">
      <span class="post-type">{{ post.type }}</span>
      <span class="post-date">{{ post.date }}</span>
    </div>
    <h3>{{ post.title }}</h3>
    <p class="desc">{{ post.description }}</p>
    <div v-if="post.attachment" class="post-attachment">
      <span>📎 <a :href="`/fisiere/${post.attachment}`" target="_blank">{{ post.attachment }}</a></span>
    </div>
    <div class="post-meta" v-if="post.author">
      <span>Postat de <b>{{ post.author }}</b></span>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    post: Object
  }
}
</script>

<style scoped>
.post-card {
  background: #fff;
  border-radius: 14px;
  box-shadow: 0 4px 18px #3ac0e510;
  padding: 22px 36px 18px 36px;
  margin-bottom: 28px;
  transition: box-shadow 0.18s;
  min-width: 350px;
  max-width: 750px;   /* mai lung */
  width: 100%;
  border: 1px solid #e0e7ef;
  font-family: 'Inter', sans-serif;
}
.post-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 4px;
  font-size: 1.05rem;
}
.post-type {
  background: #e1f0ff;
  color: #1976d2;
  font-weight: 600;
  padding: 3px 12px;
  border-radius: 8px;
  font-size: 0.98rem;
}
.post-date {
  color: #7aa4c8;
  font-size: 0.98rem;
}
h3 {
  margin: 4px 0 10px 0;
  font-size: 1.26rem;
  color: #1976d2;
  font-family: 'Inter', sans-serif;
}
.desc {
  font-size: 1.09rem;
  color: #323c4c;
  margin-bottom: 12px;
}
.post-attachment {
  margin-bottom: 12px;
  font-size: 1.01rem;
}
.post-attachment a {
  color: #1976d2;
  text-decoration: underline;
  margin-left: 2px;
}
.post-meta {
  font-size: 0.96rem;
  color: #7890a0;
  margin-top: 4px;
}
</style>
