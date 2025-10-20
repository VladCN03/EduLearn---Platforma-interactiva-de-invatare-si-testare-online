<template>
  <header class="header">
    <h1>{{ pageTitle }}</h1>
    <div class="user-mini">
      <img :src="user.avatar" alt="avatar" />
      <span>{{ user.name }}</span>
    </div>
  </header>
</template>

<script>
export default {
  props: ['user'],
  computed: {
    pageTitle() {
      // Poți să expui titlul activ aici
      return 'Platforma EduLearn'
    }
  }
}
</script>

<style scoped>
.header {
  height: 64px;
  background: #fff;
  border-bottom: 1px solid #ececec;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 36px;
  font-size: 1.3rem;
  color: #333;
}
.user-mini {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 1rem;
}
.user-mini img {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  border: 2px solid #267be8;
}
</style>
