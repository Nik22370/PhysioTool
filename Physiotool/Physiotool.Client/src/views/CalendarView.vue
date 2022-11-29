<template>
    <div class="calendar">
        <h1>Mein Kalender</h1>
        <div v-for="day in days" v-bind:key="day.timestamp">
            {{ day.germanDate }}
        </div>
    </div>
</template>

<style scoped>
</style>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            days: [],
        };
    },
    async mounted() {
        const res = await axios.get('calendar/2022/1');
        this.days = res.data;
    },
};
</script>
