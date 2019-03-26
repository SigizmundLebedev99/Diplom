<template>
    <div>
        <v-tabs
          v-model="model"
          centered
          slider-color="yellow">
          <v-tab ripple>
            {{headers[0].name}}
          </v-tab>
          <v-tab ripple>
            {{headers[1].name}}
          </v-tab>
          <v-tab ripple>
            {{headers[2].name}}
          </v-tab>
        </v-tabs>

        <v-tabs-items v-model="model">
            <v-tab-item>
                <v-card flat>
                <v-card-text>Комментарии</v-card-text>
                </v-card>
            </v-tab-item>
            <v-tab-item>
                <v-card flat>
                <v-card-text>Вложения</v-card-text>
                </v-card>
            </v-tab-item>
            <v-tab-item>
                <v-card flat>
                <v-card-text>История</v-card-text>
                </v-card>
            </v-tab-item>
        </v-tabs-items>
    </div>
</template>

<script>
export default {
    data:()=>({
        model:null,
        headers:[
            {name:'Комментарии'},
            {name:'Вложения'},
            {name:'История'},
        ]
    })
}
</script>

