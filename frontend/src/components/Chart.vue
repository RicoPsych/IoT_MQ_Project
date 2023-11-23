<template>
  <div class="backdrop" @click.self="closeAddHabit">
    <div class="container">
      <div style="display: flex; justify-content: space-between; align-items: center; margin: 10px; width:100%;">
        <button @click="prevChart" :disabled="currChartIndex === 0">prev</button>
        <h1>{{ title }}</h1>
        <button @click="nextChart" :disabled="currChartIndex === displayData.length - 1">next</button>
      </div>
      <Line id="my-chart-id" :options="chartOptions" :data="chartData" />
    </div>
  </div>
</template>

<script>
import { Line } from "vue-chartjs";
import dayjs from "dayjs";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export default {
  components: { Line },
  data() {
    return {
      currChartIndex: 0,
      title: "No data to be displayed",

      //chart settings
      chartOptions: {
        responsive: true,
        plugins: {
          legend: {
            display: false,
          },
        },
      },
    };
  },
  props: {
    data: null,
  },
  methods: {
    closeAddHabit() {
      this.$emit("closeChart");
    },
    nextChart() {
      if (this.currChartIndex < this.displayData.length - 1) this.currChartIndex++;
    },
    prevChart() {
      if (this.currChartIndex > 0) this.currChartIndex--;
    },
  },
  computed: {
    // displayData converts all data into lists of particular instance and type values
    // it changes list of measurement objects into list of Instance&Type objects
    // each containing four lists of time, instance, type and value
    displayData() {
      if(this.data.length<1) return [{ time: [], instance: [], type: [], value: [] }]
      let temp_dict = this.data.reduce((acc, obj) => {
        const key = `${obj.instance}-${obj.type}`;
        if (!acc[key]) {
          acc[key] = { time: [], instance: [], type: [], value: [] };
        }
        acc[key].time.push(dayjs(obj.time).format("DD/MM HH:mm:ss"));
        acc[key].instance.push(obj.instance);
        acc[key].type.push(obj.type);
        acc[key].value.push(obj.value);
        return acc;
      }, {});
      return Object.values(temp_dict);
    },
    // data passed to Chart component
    chartData() {
      this.title = `Device ${
        this.displayData[this.currChartIndex].instance[0]
      }'s ${this.displayData[this.currChartIndex].type[0]} (top 100 measurements)`;
      return {
        labels: this.displayData[this.currChartIndex].time.slice(0, 100),
        datasets: [
          {
            backgroundColor: "hsla(160, 100%, 37%, 1)",
            borderColor: "hsla(160, 100%, 17%, 1)",
            data: this.displayData[this.currChartIndex].value.slice(0, 100),
          },
        ],
      };
    },
  },
};
</script>

<style scoped>
.container {
  width: 80%;
  height: 80%;
  padding: 50px;
  display: flex;
  align-items: center;
  justify-content: space-evenly;
  flex-direction: column;
  border: 1px solid hsla(160, 100%, 17%, 1);
  background: var(--color-background);
}

.backdrop {
  top: 0;
  left: 0;
  position: fixed;
  background: rgba(0, 0, 0, 0.65);
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 99;
}

button {
  margin: 0px;
  height: 50px;
  border: 2px solid hsla(160, 100%, 17%, 1);
  min-width: 50px;
  background-color: var(--color-background);
  font-size: 1rem;
  color: var(--color-text);
  cursor: pointer;
  border-radius: 50px;
  box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2);
}

button:disabled {
  opacity:0.3;
}

button:enabled:hover {
  border: 2px solid var(--color-accent);
}

</style>